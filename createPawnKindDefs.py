import xml.etree.ElementTree as ET
import os

nonHumanPawnKinds = []
# if you find a non human don't add it to the final patch
nonHumanExceptionList = ['Norbal', 'Human', 'OG_Human_Mechanicus']

def updateNonHumanWatchList(pdef: ET.Element):
    raceEle = pdef.find('race')
    defName = pdef.find('defName')
    attribName = pdef.attrib.get('Name')
    if raceEle != None and raceEle.text != None and raceEle.text not in nonHumanExceptionList:
        defNameText = ''
        attribNameValue = ''
        if defName != None:
            defNameText = defNameText
        else:
            attribNameValue = attribName
        nonHumanPawnKinds.append({
            'name': attribNameValue,
            'defName': defNameText,
            'race': raceEle.text
        })
        return True
    return False

pawnKindsFound = {}

def getSteamAppsPath():
    return 'C:\\Program Files (x86)\\Steam\\steamapps'

def getRimworldDataModsPath():
    return getSteamAppsPath()+'\\common\\RimWorld\\Data'

def getWorkshopModsPath(modID):
    return getSteamAppsPath()+'\\workshop\\content\\294100\\'+modID

def getPersonalPath(folderName: str):
    return 'C:\\Program Files (x86)\\Steam\\steamapps\\common\\Rimworld\\Mods\\'+folderName

def getPersonalPawnKindDefsPath(folderName):
    return getPersonalPath(folderName)+'\\Defs\\PawnKindDefs'

def getPersonalPatchesPath(folderName):
    return getPersonalPath(folderName)+'\\Patches'

def createPKD(
    destinationModPrefix: str,
    destinationModFolderName: str,
    sourcePawnKindDefsFileNames,
    speciesArray,
    sourceModID: str,
    pawnKindDefNamesArray,
    possibleParentsInc,
    includeBackstories=True
):
    pawnKindsFound = pawnKindDefNamesArray
    possibleParents = possibleParentsInc
    outputFiles = {}
    # Create the PAWN KIND DEF
    for file in sourcePawnKindDefsFileNames:
        outputFiles[file] = {'abstracts': ET.Element('Defs'), 'nonabstracts': ET.Element('Defs')}
        pawnKDTree = None
        if sourceModID in ['Core', 'Ideology', 'Royalty', 'Biotech', 'Anamoly']:
            fileName = getRimworldDataModsPath()+'\\'+sourceModID+'\\'+file+'.xml'
        else:
            fileName = getWorkshopModsPath(sourceModID)+file+'.xml'
            
        pawnKDTree = ET.parse(fileName)
        if pawnKDTree == None:
            raise 'file '+fileName+' did not import'
        root = pawnKDTree.getroot()
        for pawnKindDef in root.findall('PawnKindDef'):
            nameOrDefName, defNameValue, attribName = getNameOrDefName(pawnKindDef)
            attribAbstract = pawnKindDef.attrib.get('Abstract')
            if attribAbstract == 'True':
                possibleParents[attribName] = pawnKindDef
            else:
                if attribName != None:
                    possibleParents[attribName] = pawnKindDef
                pawnKindsFound[nameOrDefName] = {'element': pawnKindDef, 'source': sourceModID, 'filename': file}
    for key in pawnKindsFound.keys():
        pawnKind = pawnKindsFound[key]['element']

        nameOrDefName, defNameValue, attribName = getNameOrDefName(pawnKind)

        if updateNonHumanWatchList(pawnKind):
            continue
        # check if has a parent
        pawnKind = getParentStuff(pawnKind, possibleParents)

        if pawnKind == None:
            continue

        # get the label - required
        labelText = None
        labelEle = pawnKind.find('label')
        if labelEle != None:
            labelText = labelEle.text

        newPKD = ET.Element('PawnKindDef', {
                            'Abstract': 'True', 'Name': destinationModPrefix+'_'+nameOrDefName})

        age = ET.Element('minGenerationAge')
        age.text = "17"
        backstoryprefix = destinationModPrefix+'Backstory'
        parentName = pawnKind.attrib.get('ParentName')
        if parentName != None:
            newPKD.attrib['ParentName'] = parentName
        for ele in pawnKind:
            if ele == None or ele.tag == None or ele.tag in ['xenotypeSet', 'backstoryFiltersOverride']:
                continue
            if ele.tag == 'minGenerationAge':
                if ele.text != None and int(ele.text) > int(age.text):
                    age.text = ele.text
                continue
            if includeBackstories == True and ele.tag in ['backstoryCategories', 'backstoryFilters']:
                continue
            newPKD.append(ele)
        if includeBackstories == True:
            newBackstoryFiltersOverride = ET.Element('backstoryFiltersOverride',
                                                     {'Inherit': 'False'})
            newBackstoryFiltersOverride.append(categoryTags(
                backstoryprefix, newPKD.find('defaultFactionType'), ET.Element('li')))
            newPKD.append(newBackstoryFiltersOverride)
        defsClass = None
        if pawnKind.attrib.get('Class') != None:
            defsClass = pawnKind.attrib.get('Class')

        # update the defname - not required
        if defNameValue != None and newPKD.find('defName') != None:
            newPKD.remove(newPKD.find('defName'))
        pawnKindsFound[key]['element'] = newPKD

        newPKD.append(age)
        if pawnKindsFound[key]['source'] == sourceModID and pawnKindsFound[key]['filename'] in outputFiles.keys():
            outputFiles[pawnKindsFound[key]['filename']
                        ]['abstracts'].append(newPKD)
            # add a pawnkind entry for each species to the DEFS
            nonabs = outputFiles[pawnKindsFound[key]['filename']]['nonabstracts']
            for speciesEntry in speciesArray:
                file = createSpeciesFile(speciesEntry, newPKD, labelText, nameOrDefName, defsClass, nonabs, sourceModID)
                outputFiles[pawnKindsFound[key]['filename']]['nonabstracts'] = file
    for file in outputFiles.keys():
        if len(outputFiles[file]['abstracts'].findall('PawnKindDef')) > 0:
            outputPawnKindFile(
                outputFiles[file]['abstracts'], destinationModFolderName, file+'_Abstracts', sourceModID)
        if len(outputFiles[file]['nonabstracts'].findall('PawnKindDef')) > 0:
            outputPawnKindFile(
                outputFiles[file]['nonabstracts'], destinationModFolderName, file, sourceModID)

    non = []
    for key in pawnKindsFound.keys():
        for n in nonHumanPawnKinds:
            if 'defName' in n.keys():
                if n['defName'] != None and n['defName'] == key:
                    non.append(key)
                    continue
            if 'name' in n.keys():
                if n['name'] != None and n['name'] == key:
                    non.append(key)
                    continue
    for n in non:
        try:
            pawnKindsFound.pop(n)
        except:
            print(n)
    return pawnKindsFound, possibleParents

def outputPawnKindFile(defStructure, destinationModFolderName, sourceFile, filePrefix):
    ET.indent(defStructure)
    newTree = ET.ElementTree(defStructure)
    folder = getPersonalPawnKindDefsPath(
        destinationModFolderName) 
    x = folder+ '\\' + \
        filePrefix + sourceFile[find_last_index(
            sourceFile, '\\')+1:len(sourceFile)]+'.xml'
    
    
    if not os.path.exists(folder):
        os.makedirs(folder)
    if os.path.exists(x):
        os.remove(x)

    try:
        newTree.write(x, 'utf-8', 'true')
    except:
        print(ET.dump(defStructure))
        raise

def categoryTags(backstoryprefix, factionEle, li: ET.Element):
    cat = ET.Element('categories')
    catli = ET.Element('li')
    catli.text = backstoryprefix
    cat.append(catli)
    if factionEle == None or factionEle.text == None:
        catli = ET.Element('li')
        catli.text = backstoryprefix+'Outlander'
        cat.append(catli)
    else:
        factionText = factionEle.text
        if factionText in ['TribalCivil', 'TribeRough', 'TribeSavage', 'PlayerTribe', 'NorbalTribe', 'VFEM_KingdomCivil', 'VFEM_PlayerKingdom', 'VFEV_PlayerClan', 'VFEV_VikingsSlaver', 'VFEV_VikingsClan']:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Tribal'
            cat.append(catli)
        elif factionText in ['RWP_Faction_Bandit', 'RWP_Faction_Marshal', 'RWP_Faction_Town', 'VFES_PlayerSettlement']:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Outlander'
            cat.append(catli)
        elif factionText in ['PlayerColony', 'Ancients', 'AncientsHostile', 'PJ_GalacticEmpire', 'PJ_RebelFac', 'PJ_Bounty', 'FirstOrder']:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Offworld'
            cat.append(catli)
        elif factionText in ['OutlanderCivil', 'OutlanderRough', 'VFEA_AncientSoldiers', 'ROM_Townsfolk', 'ROM_TheAgency', 'PTPsionic', 'Seers']:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Outlander'
            cat.append(catli)
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Offworld'
            cat.append(catli)
        elif factionText in ['Pirate']:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Pirate'
            cat.append(catli)
        elif factionText in ['BlueMoonCorpSW', 'ROM_LostPlatoon', 'Polaribloc_SecuirityForce', 'FPC', 'Feral']:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Outlander'
            cat.append(catli)
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Offworld'
            cat.append(catli)
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Pirate'
            cat.append(catli)
        else:
            catli = ET.Element('li')
            catli.text = backstoryprefix+'Outlander'
            cat.append(catli)
    li.append(cat)
    return li

def find_last_index(search_list, search_item):
    return len(search_list) - 1 - search_list[::-1].index(search_item)

def createSpeciesFile(speciesEntry, newPKD: ET.Element, labelText, nameOrDefName, defsClass, SPECIESDEFS, sourceModID):
    for race in speciesEntry['race']:
        pawnKindBeingCreated = ET.Element('PawnKindDef', { 'ParentName': newPKD.attrib.get('Name') })
        ele = ET.Element('race')
        ele.text = race
        pawnKindBeingCreated.append(ele)
        ele = ET.Element('label')
        ele.text = speciesEntry['label'] + \
            ' ' + (labelText or 'drifter')
        pawnKindBeingCreated.append(ele)
        defName = nameOrDefName
        ele = ET.Element('defName')
        ele.text = race+'_'+defName
        pawnKindBeingCreated.append(ele)

        if ('Colonist' in defName) or ('Tribesperson' in defName):
            if 'apparelRequired' in speciesEntry and len(speciesEntry['apparelRequired']) > 0:
                ele = ET.Element('apparelRequired')
                for apparel in speciesEntry['apparelRequired']:
                    li = ET.Element('li')
                    li.text = apparel
                    ele.append(li)
                pawnKindBeingCreated.append(ele)
        if 'apparelTags' in speciesEntry and len(speciesEntry['apparelTags']) > 0:
            ele = ET.Element('apparelTags')
            for tag in speciesEntry['apparelTags']:
                li = ET.Element('li')
                li.text = tag
                ele.append(li)
            pawnKindBeingCreated.append(ele)
        if sourceModID != 'Biotech' and 'xenoType' in speciesEntry and len(speciesEntry['xenoType']) > 0:
            xenotypeSet = ET.Element('xenotypeSet')

            xenotypeChances = ET.Element('xenotypeChances')
            xenotypeChances.attrib['Inherit'] = "False"
            for type in speciesEntry['xenoType']:
                li = ET.Element(type['label'])
                li.text = type['modifier']
                li.attrib['MayRequire']="Ludeon.RimWorld.Biotech"
                xenotypeChances.append(li)
            xenotypeSet.append(xenotypeChances)
            pawnKindBeingCreated.append(xenotypeSet)
        if defsClass != None:
            pawnKindBeingCreated.attrib['Class'] = defsClass
        ET.indent(pawnKindBeingCreated, '  ', 1)
        SPECIESDEFS.append(pawnKindBeingCreated)
    return SPECIESDEFS

def getParentStuff(pawnKindDef: ET.Element, possibleParents):
    currentParentName = pawnKindDef.attrib.get('ParentName')
    if currentParentName == None or currentParentName == '':
        return pawnKindDef

    parents = [pawnKindDef]
    try:
        currentParentEle: ET.Element = possibleParents[currentParentName]
    except:
        return pawnKindDef
    while currentParentEle != None:
        if currentParentName == None or currentParentName == '':
            currentParentEle = None
            continue
        currentParentEle: ET.Element = possibleParents[currentParentName]
        if currentParentEle != None:
            currentParentName = currentParentEle.attrib.get('ParentName')
            parents.append(currentParentEle)

    newPawnKD = ET.Element('PawnKindDef')
    for parent in parents[::-1]:
        classValue = parent.attrib.get('Class')
        if classValue != None:
            newPawnKD.attrib['Class'] = classValue
        for ele in parent:
            if ele.tag == 'race' and ele.text not in nonHumanExceptionList:
                updateNonHumanWatchList(parent)
                return None
            if ele.tag == 'minGenerationAge':
                continue
            if ele.tag == 'backstoryCategories':
                continue
            if ele.tag == 'backstoryFilters':
                continue
            if ele.tag == 'backstoryFiltersOverride':
                continue
            if ele.tag == 'defName':
                continue
            existing = newPawnKD.find(ele.tag)
            if existing != None:
                if ele.text != None:
                    if existing.text != None and ele.text == existing.text:
                        continue
                    newPawnKD.remove(existing)
                else:
                    newPawnKD.remove(existing)
            newPawnKD.append(ele)
    return newPawnKD

def getNameOrDefName(pawnKindDef):
    nameOrDefName = None
    defName = pawnKindDef.find('defName')
    defNameValue = None
    if defName != None:
        defNameValue = defName.text
        nameOrDefName = defNameValue

    attribName = pawnKindDef.attrib.get('Name')
    if attribName != None:
        if nameOrDefName == None:
            nameOrDefName = attribName
    return nameOrDefName, defNameValue, attribName
