# importing the module.
import xml.etree.ElementTree as ET
import os


def getSteamAppsPath():
    return 'C:\\Program Files (x86)\\Steam\\steamapps'
    # return 'C:\\MyPrograms\\Steam\\steamapps'


# CORE MOD
def getRimworldDataModsPath():
    return getSteamAppsPath()+'\\common\\RimWorld\\Data'


def getWorkshopModsPath(modID):
    return getSteamAppsPath()+'\\workshop\\content\\294100\\'+modID


def getPersonalPath(folderName: str):
    return os.getcwd() + '\\' + folderName



def getPersonalPawnKindDefsPath(folderName):
    return getPersonalPath(folderName)+'\\Defs\\PawnKindDefs'


def getPersonalPatchesPath(folderName):
    return getPersonalPath(folderName)+'\\Patches'


def createFactionPatch(destinationFolder, factionFileNames, sourceModID, speciesArray, pawnKinds):
    #  Create the PATCH to add the groups to the FACTION DEF
    for file in factionFileNames:
        if sourceModID in ['Core', 'Ideology', 'Royalty', 'Biotech', 'Anamoly']:
            factionTree = ET.parse(
                getRimworldDataModsPath()+'\\'+sourceModID+'\\'+file+'.xml')
        else:
            factionTree = ET.parse(getWorkshopModsPath(
                sourceModID) + '\\'+file+'.xml')
        # copy get all FactnDefs into a list
        root = factionTree.getroot()
        for factionDef in root.findall('FactionDef'):
            if factionDef.find('pawnGroupMakers') == None:
                continue

            pgm: ET.Element = factionDef.findall('pawnGroupMakers/li')
            if pgm == None:
                raise 'triple fuck'

            for originalGroup in pgm:
                if originalGroup == None:
                    raise '4x fuck'
                createGroups(pawnKinds,
                             originalGroup, speciesArray, factionDef, destinationFolder, file)


def addRaces(item, opt, newOptions):
    for race in item['race']:
        a = ET.Element(race+'_'+opt.tag)
        mayRequire = opt.attrib.get('MayRequire')
        if mayRequire != None:
            a.attrib['MayRequire'] = mayRequire
        a.text = '{0:.2f}'.format(
            (float(opt.text) * item['modifier'])/len(item['race']))
        if float(a.text) < 0.001:
            a.text = '0.001'
        newOptions.append(a)
    return newOptions


def setupCombatGroup(list: ET.Element, originalGroup: ET.Element, typeOfGroup='Combat'):
    disallowedStrat = originalGroup.find('disallowedStrategies')
    commonality = originalGroup.find('commonality')
    kindDef = ET.Element('kindDef')
    kindDef.text = typeOfGroup
    list.append(kindDef)
    if disallowedStrat != None:
        list.append(disallowedStrat)
    comm = ET.Element('commonality')
    if commonality == None:
        comm.text = '20'
    elif commonality.text == None:
        comm.text = '20'
    else:
        comm.text = '{0:.2f}'.format(float(commonality.text)*0.2)
    list.append(comm)
    return list


def prepPeacefulGroup(list: ET.Element, typeOfGroup: str):
    kindDef = ET.Element('kindDef')
    kindDef.text = typeOfGroup
    list.append(kindDef)
    return list


def prepTraderGroup(list: ET.Element, originalGroup: ET.Element):
    kindDef = ET.Element('kindDef')
    kindDef.text = 'Trader'
    list.append(kindDef)
    # todo add one trader per species
    list.append(originalGroup.find('traders'))
    # never replace carriers
    list.append(originalGroup.find('carriers'))
    return list


def factionPawnGroupMaker(pawnKinds, valueEle: ET.Element, originalGroup: ET.Element, speciesEntry, typeOfGroup='Combat', mayRequire=None):
    options = 'options'
    if typeOfGroup == 'Trader':
        options = 'guards'
    existingOptions = originalGroup.find(options)
    if existingOptions == None:
        for x in originalGroup:
            print(x.text)
        raise 'no options found with tag ' + options

    list = ET.SubElement(valueEle, 'li')
    if mayRequire != None:
        list.attrib.setdefault('MayRequire', mayRequire)
    # copy the existing with mix of all species
    list = setupGroupByType(list, originalGroup, typeOfGroup)

    newOptions = ET.SubElement(list, options)
    # add human and nonhuman pawnkinds already in the group only once /as is/
    # then add my races to make this a mixed group
    addedAtLeastOneOfMine = False
    for opt in existingOptions:
        newOptions.append(opt)
    for opt in existingOptions:
        if opt.tag not in pawnKinds:
            continue
        addedAtLeastOneOfMine = True
        for entry in speciesEntry:
            newOptions = addRaces(entry, opt, newOptions)

    if len(newOptions) < 1 or addedAtLeastOneOfMine == False:
        valueEle.remove(list)

    # add a group that is only my races
    list = ET.SubElement(valueEle, 'li')
    if mayRequire != None:
        list.attrib.setdefault('MayRequire', mayRequire)
    list = setupGroupByType(list, originalGroup, typeOfGroup)

    newOptions = ET.SubElement(list, options)

    for entry in speciesEntry:
        for opt in existingOptions:
            if opt.tag not in pawnKinds:
                continue
            newOptions = addRaces(entry, opt, newOptions)

    if len(newOptions) < 1:
        valueEle.remove(list)

    # add a group that is only one of my races
    for entry in speciesEntry:
        list = ET.SubElement(valueEle, 'li')
        if mayRequire != None:
            list.attrib.setdefault('MayRequire', mayRequire)
        list = setupGroupByType(list, originalGroup, typeOfGroup)

        newOptions = ET.SubElement(list, options)

        for opt in existingOptions:
            if opt.tag not in pawnKinds:
                continue
            newOptions = addRaces(entry, opt, newOptions)

        if len(newOptions) < 1:
            valueEle.remove(list)

    return valueEle


def setupGroupByType(list, originalGroup, typeOfGroup):
    if typeOfGroup == 'Combat':
        return setupCombatGroup(list, originalGroup, typeOfGroup)
    elif typeOfGroup in ['Hunters',  'Farmers', 'Miners', 'Loggers']:
        return setupCombatGroup(list, originalGroup, typeOfGroup)
    elif typeOfGroup == 'Trader':
        return prepTraderGroup(list, originalGroup)
    else:
        return prepPeacefulGroup(list, typeOfGroup)


def createGroups(pawnKinds, originalGroup, speciesEntry, factionDef, destinationFolder, file):
    typeOfGroup = 'Combat'
    kindDefEle = originalGroup.find('kindDef')
    if kindDefEle != None:
        typeOfGroup = kindDefEle.text
    valueEle = ET.Element('value')
    if typeOfGroup in ['Combat']:
        valueEle = factionPawnGroupMaker(
            pawnKinds,  valueEle, originalGroup, speciesEntry, typeOfGroup)

    elif typeOfGroup in ['Hunters',  'Farmers', 'Miners', 'Loggers']:
        valueEle
        valueEle = factionPawnGroupMaker(
            pawnKinds, valueEle, originalGroup, speciesEntry, typeOfGroup, 'Ludeon.RimWorld.Ideology')

    elif typeOfGroup == 'Trader':
        valueEle = factionPawnGroupMaker(
            pawnKinds,  valueEle, originalGroup, speciesEntry, typeOfGroup)
    else:
        valueEle = factionPawnGroupMaker(
            pawnKinds,  valueEle, originalGroup, speciesEntry, typeOfGroup)

    nameOrDefName = ''
    newPatch = ET.Element('Patch')
    # don't add operation for empty group
    if valueEle != None and len(valueEle.findall('li')) > 0:
        # an ADD operation is created for the group maker
        newOperation = ET.SubElement(newPatch, 'Operation', {
            'Class': "PatchOperationAdd"})
        xpathEle = ET.Element('xpath')
        defname = factionDef.find('defName')
        if defname != None:
            nameOrDefName = defname.text
            xpathEle.text = '/Defs/FactionDef[defName="' + \
                defname.text+'"]/pawnGroupMakers'
        else:
            nameOrDefName = factionDef.attrib['Name']
            xpathEle.text = '/Defs/FactionDef[@Name="' + \
                nameOrDefName+'"]/pawnGroupMakers'
        newOperation.append(xpathEle)
        newOperation.append(valueEle)
    else:
        print('no li items found')
    writeToFile(newPatch, destinationFolder, file +
                '_'+nameOrDefName+'_'+typeOfGroup)


def find_last_index(search_list, search_item):
    return len(search_list) - 1 - search_list[::-1].index(search_item)


def writeToFile(newPatch, destFolder, fileName):
    # don't add file for empty patch
    if len(newPatch.findall('Operation')) < 1:
        return
    # format tree
    ET.indent(newPatch)
    newTree = ET.ElementTree(newPatch)
    x = getPersonalPatchesPath(
        destFolder) + '\\' + \
        fileName[find_last_index(
            fileName, '\\')+1:len(fileName)]+'.xml'
    # write file and folder if needed
    try:
        newTree.write(x, 'utf-8', 'true')
    except OSError as error:
        os.makedirs(destFolder)
        newTree.write(x, 'utf-8', 'true')
