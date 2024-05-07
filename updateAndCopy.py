from createPawnKindDefs import createPKD
from factionPatchCreate import createFactionPatch
import sys
sys.path.append('..')

pawnKindDefNamesArray = {}
possibleParents = {}

modPrefix = 'StarWarsRaces'
destinationPathVersion = '1.5'
otherModsPath = 'MayRequire'

destinationPathVersion = '1.5'
species = [
    {
        'race': ['StarWarsRaces_Wookiee'],
        'label':'Wookie',
        'modifier':0.7,
        'apparelRequired':['StarWarsRaces_Wookiee_Bandolier_Tribalwear'],
        'apparelTags':['StarWarsRaces_WookieeApparelTag'],
        "xenoType": [{"label": "StarWarsRaces_WookieeBaseliner", "modifier": "999"}],
    },
    {
        'race': ['StarWarsRaces_Togruta'],
        'label':'Togruta',
        'modifier':1,
        'apparelRequired':[],
        'apparelTags':[],
        "xenoType": [{"label": "StarWarsRaces_TogrutaBaseliner", "modifier": "999"}],
    },
    {
        'race': ['StarWarsRaces_Rodian'],
        'label':'Rodian',
        'modifier':1,
        'apparelRequired':[],
        'apparelTags':[],
        "xenoType": [{"label": "StarWarsRaces_RodianBaseliner", "modifier": "999"}],
    },
    {
        'race': ['StarWarsRaces_Ewok'],
        'label':'Ewok',
        'modifier':0.5,
        'apparelRequired':['StarWarsRaces_Ewok_Hood', 'StarWarsRaces_Ewok_Parka'],
        'apparelTags':['StarWarsRaces_EwokApparelTag'],
        "xenoType": [{"label": "StarWarsRaces_EwokBaseliner", "modifier": "999"}],
    },
    {
        'race': ['StarWarsRaces_Twilek'],
        'label':'Twi\'lek',
        'modifier':1,
        'apparelRequired':['StarWarsRaces_TwilekHeadCovering'],
        'apparelTags':['StarWarsRaces_TwilekApparelTag'],
        "xenoType": [{"label": "StarWarsRaces_TwilekBaseliner", "modifier": "999"}],
    },
    {
        'race': ['StarWarsRaces_MonCalamari'],
        'label':'Mon Calamari',
        'modifier':0.5,
        'apparelRequired':[],
        'apparelTags':[],
        "xenoType": [{"label": "StarWarsRaces_MonCalamariBaseliner", "modifier": "999"}],
    }
]


# ================================Core========================================
# ================================Core========================================
# ================================Core========================================
coreModID = 'Core'
pawnKindDefsFiles = [
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Mercenary',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Outlander',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Pirate',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Player',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Spacer',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Special',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Tribal',
    '\\Defs\\PawnKinds\\PawnKinds_Breach',
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    destinationPathVersion,
    pawnKindDefsFiles,
    species,
    coreModID,
    pawnKindDefNamesArray,
    possibleParents,
    False
)


# ================================Ideology========================================
# ================================Ideology========================================
# ================================Ideology========================================


ideologyModID = 'Ideology'
pawnKindDefsFiles = [
    '\\Defs\\PawnKinds\\PawnKinds_NeutralCamps',
    '\\Defs\\PawnKinds\\PawnKinds_Special',
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\'+ideologyModID+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    ideologyModID,
    pawnKindDefNamesArray,
    possibleParents,
    False
)
# ================================Royalty========================================
# ================================Royalty========================================
# ================================Royalty========================================


royaltyModId = 'Royalty'
pawnKindDefsFiles = [
    '\\Defs\\PawnKinds\\PawnKinds_Empire',
    '\\Defs\\PawnKinds\\PawnKinds_Refugee',
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\'+royaltyModId+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    royaltyModId,
    pawnKindDefNamesArray,
    possibleParents,
    False
)
# ================================Biotech========================================
# ================================Biotech========================================
# ================================Biotech========================================


biotechModId = 'Biotech'
pawnKindDefsFiles = [
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Special',
    '\\Defs\\PawnKindDefs_Humanlikes\\PawnKinds_Waster',
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\'+biotechModId+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    biotechModId,
    pawnKindDefNamesArray,
    possibleParents,
    False
)

# # ================================WH40K========================================
# # ================================WH40K========================================
# # ================================WH40K========================================

# modIDFaction_40kAstraMilitarum = '1541499916'
# destinationFolderNameFaction_40kAstraMilitarum = 'Faction_40kAstraMilitarum'
# pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
# pawnKindDefsFiles = [
#     pawnKindFolder+'OG_AMAXB_PawnKinds_ImperialGuard_Abstract',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_ImperialGuard_Cadian',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_ImperialGuard_Player',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_ImperialGuard_Special',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_ImperialGuard',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_Mechanicus_Abstract',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_Mechanicus_Player',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_Mechanicus_Special',
#     pawnKindFolder+'OG_AMAXB_PawnKinds_Mechanicus'
# ]

# pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
#     modPrefix,
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_40kAstraMilitarum+'\\'+destinationPathVersion,
#     pawnKindDefsFiles,
#     species,
#     modIDFaction_40kAstraMilitarum,
#     pawnKindDefNamesArray,
#     possibleParents,
#     False
# )

# # ================================Ancients========================================
# # ================================Ancients========================================
# # ================================Ancients========================================
modIDFaction_Ancients = '2654846754'
destinationFolderNameFaction_Ancients = 'Faction_Ancients'
pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs_Humanlikes\\'
pawnKindDefsFiles = [
    pawnKindFolder+'PawnKinds_Player',
    pawnKindFolder+'PawnKinds_Spacer'
]

pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\' +
    destinationFolderNameFaction_Ancients+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    modIDFaction_Ancients,
    pawnKindDefNamesArray,
    possibleParents,
    False
)

# # ================================BlueMoon========================================
# # ================================BlueMoon========================================
# # ================================BlueMoon========================================

modIDFaction_BlueMoon = '1123043922'
destinationFolderNameFaction_BlueMoon = 'Faction_BlueMoon'
pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs_Humanlikes\\'
pawnKindDefsFiles = [
    pawnKindFolder+'PawnKinds_BlueSunSoldiers_SparklingWorlds'
]

pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\' +
    destinationFolderNameFaction_BlueMoon+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    modIDFaction_BlueMoon,
    pawnKindDefNamesArray,
    possibleParents,
    False
)


# # ================================PolarisBloc========================================
# # ================================PolarisBloc========================================
# # ================================PolarisBloc========================================

# modIDFaction_PolarisBloc = '1500248421'
# destinationFolderNameFaction_PolarisBloc = 'Faction_PolarisBloc'
# pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
# pawnKindDefsFiles = [
#     pawnKindFolder+'PawnKinds_SecuirityForce'
# ]

# pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
#     modPrefix,
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_PolarisBloc+'\\'+destinationPathVersion,
#     pawnKindDefsFiles,
#     species,
#     modIDFaction_PolarisBloc,
#     pawnKindDefNamesArray,
#     possibleParents,
#     False
# )
# # ================================PsiTech========================================
# # ================================PsiTech========================================
# # ================================PsiTech========================================

# modIDFaction_PsiTech = '2078892294'
# destinationFolderNameFaction_PsiTech = 'Faction_PsiTech'
# pawnKindFolder = '\\v1.4\\Defs\\PawnKindDefs\\'
# pawnKindDefsFiles = [
#     pawnKindFolder + 'PawnKinds_PsiTech'
# ]
# pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
#     modPrefix,
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_PsiTech+'\\'+destinationPathVersion,
#     pawnKindDefsFiles,
#     species,
#     modIDFaction_PsiTech,
#     pawnKindDefNamesArray,
#     possibleParents,
#     False
# )
# # ================================RimsenalFed========================================
# # ================================RimsenalFed========================================
# # ================================RimsenalFed========================================

modIDFaction_RimsenalFederation = '736172213'
destinationFolderNameFaction_RimsenalFederation = 'Faction_RimsenalFederation'
pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
pawnKindDefsFiles = [
    pawnKindFolder + 'PawnKinds_Aux'
]

pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\' +
    destinationFolderNameFaction_RimsenalFederation+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    modIDFaction_RimsenalFederation,
    pawnKindDefNamesArray,
    possibleParents,
    False
)
# # ================================RimsenalFeral========================================
# # ================================RimsenalFeral========================================
# # ================================RimsenalFeral========================================

modIDFaction_RimsenalFeral = '736207111'
destinationFolderNameFaction_RimsenalFeral = 'Faction_RimsenalFeral'
pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
pawnKindDefsFiles = [
    pawnKindFolder + 'PawnKinds_Feral'
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\' +
    destinationFolderNameFaction_RimsenalFeral+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    modIDFaction_RimsenalFeral,
    pawnKindDefNamesArray,
    possibleParents,
    False
)


# ================================StarWarsFactions========================================
# ================================StarWarsFactions========================================
# ================================StarWarsFactions========================================

modIDFaction_SWFactions = '918227266'
destinationFolderNameFaction_SWFactions = 'Faction_SWFactions'
pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
pawnKindDefsFiles = [
    pawnKindFolder + 'PawnKinds_Imp',
    pawnKindFolder + 'PawnKinds_Rebel',
    pawnKindFolder + 'PawnKinds_Scum'
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\' +
    destinationFolderNameFaction_SWFactions+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    modIDFaction_SWFactions,
    pawnKindDefNamesArray,
    possibleParents,
    False
)

# # ================================StarWarsFirstOrder========================================
# # ================================StarWarsFirstOrder========================================
# # ================================StarWarsFirstOrder========================================

# modIDFaction_SWFirstOrder = '2395173388'
# destinationFolderNameFaction_SWFirstOrder = 'Faction_SWFirstOrder'
# pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
# pawnKindDefsFiles = [
#     pawnKindFolder + 'PawnKinds_FROR',
#     pawnKindFolder + 'PawnKinds_Player_FROR',
# ]
# pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
#     modPrefix,
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_SWFirstOrder+'\\'+destinationPathVersion,
#     pawnKindDefsFiles,
#     species,
#     modIDFaction_SWFirstOrder,
#     pawnKindDefNamesArray,
#     possibleParents,
#     False
# )

# # ================================VEF Settlers========================================
# # ================================VEF Settlers========================================
# # ================================VEF Settlers========================================

modIDFaction_VESettlers = '2052918119'
destinationFolderNameFaction_VESettlers = 'Faction_VESettlers'
pawnKindFolder = '\\1.4\\Defs\\PawnKindDefs\\'
pawnKindDefsFiles = [
    pawnKindFolder + 'PawnKinds_Bandits',
    pawnKindFolder + 'PawnKinds_Settlers',
    pawnKindFolder + 'PawnKinds_Player',
]
pawnKindDefNamesArray, pawnKindDefsArray = createPKD(
    modPrefix,
    otherModsPath+'\\' +
    destinationFolderNameFaction_VESettlers+'\\'+destinationPathVersion,
    pawnKindDefsFiles,
    species,
    modIDFaction_VESettlers,
    pawnKindDefNamesArray,
    possibleParents,
    False
)

# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================
# ================================FACTIONS========================================

# ================================CORE========================================
# ================================CORE========================================
# ================================CORE========================================

factionFileNames = [
    '\\Defs\\FactionDefs\\Factions_Misc'
]
createFactionPatch(
    destinationPathVersion,
    factionFileNames,
    coreModID,
    species,
    pawnKindDefNamesArray
)

# ================================Ideology========================================
# ================================Ideology========================================
# ================================Ideology========================================
factionFileNames = [
    '\\Defs\\FactionDefs\\Factions_Misc'
]
createFactionPatch(
    otherModsPath+'\\'+ideologyModID+'\\'+destinationPathVersion,
    factionFileNames,
    ideologyModID,
    species,
    pawnKindDefNamesArray
)
# ================================Royalty========================================
# ================================Royalty========================================
# ================================Royalty========================================
factionFileNames = [
    '\\Defs\\FactionDefs\\Faction_Empire',
    '\\Defs\\FactionDefs\\Factions_Misc',
]
createFactionPatch(
    otherModsPath+'\\'+royaltyModId+'\\'+destinationPathVersion,
    factionFileNames,
    royaltyModId,
    species,
    pawnKindDefNamesArray
)

# # ================================Ancients========================================
# # ================================Ancients========================================
# # ================================Ancients========================================

factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
factionFileNames = [
    factionDefFolder+'Factions_Hidden',
    factionDefFolder+'Factions_Player'
]
createFactionPatch(
    otherModsPath+'\\' +
    destinationFolderNameFaction_Ancients+'\\'+destinationPathVersion,
    factionFileNames,
    modIDFaction_Ancients,
    species,
    pawnKindDefNamesArray
)

# # ================================BlueMoon========================================
# # ================================BlueMoon========================================
# # ================================BlueMoon========================================

factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
factionFileNames = [
    factionDefFolder+'Factions_BlueMoonCorp_SparklingWorlds'
]

createFactionPatch(
    otherModsPath+'\\' +
    destinationFolderNameFaction_BlueMoon+'\\'+destinationPathVersion,
    factionFileNames,
    modIDFaction_BlueMoon,
    species,
    pawnKindDefNamesArray
)

# # ================================PolarisBloc========================================
# # ================================PolarisBloc========================================
# # ================================PolarisBloc========================================
# factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
# factionFileNames = [
#     factionDefFolder+'Factions_SecuirityForce'
# ]


# createFactionPatch(
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_PolarisBloc+'\\'+destinationPathVersion,
#     factionFileNames,
#     modIDFaction_PolarisBloc,
#     species,
#     pawnKindDefNamesArray
# )
# # ================================PsiTech========================================
# # ================================PsiTech========================================
# # ================================PsiTech========================================
# factionDefFolder = '\\v1.4\\Defs\\FactionDefs\\'
# factionFileNames = [
#     factionDefFolder+'Factions_PsiTech'
# ]


# createFactionPatch(
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_PsiTech+'\\'+destinationPathVersion,
#     factionFileNames,
#     modIDFaction_PsiTech,
#     species,
#     pawnKindDefNamesArray
# )

# # ================================RimsenalFed========================================
# # ================================RimsenalFed========================================
# ================================RimsenalFed========================================
factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
factionFileNames = [
    factionDefFolder+'FedFaction'
]


createFactionPatch(
    otherModsPath+'\\' +
    destinationFolderNameFaction_RimsenalFederation+'\\'+destinationPathVersion,
    factionFileNames,
    modIDFaction_RimsenalFederation,
    species,
    pawnKindDefNamesArray
)
# # ================================RimsenalFeral========================================
# # ================================RimsenalFeral========================================
# ================================RimsenalFeral========================================
factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
factionFileNames = [
    factionDefFolder+'Factions_Feral'
]


createFactionPatch(
    otherModsPath+'\\' +
    destinationFolderNameFaction_RimsenalFeral+'\\'+destinationPathVersion,
    factionFileNames,
    modIDFaction_RimsenalFeral,
    species,
    pawnKindDefNamesArray
)
# ================================StarWarsFactions========================================
# ================================StarWarsFactions========================================
# ================================StarWarsFactions========================================

factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
factionFileNames = [
    factionDefFolder+'Faction_Imperial',
    factionDefFolder+'Faction_Rebel',
    factionDefFolder+'Faction_Scum'
]

createFactionPatch(
    otherModsPath+'\\' +
    destinationFolderNameFaction_SWFactions+'\\'+destinationPathVersion,
    factionFileNames,
    modIDFaction_SWFactions,
    species,
    pawnKindDefNamesArray
)


# # ================================StarWarsFirstOrder========================================
# # ================================StarWarsFirstOrder========================================
# # ================================StarWarsFirstOrder========================================

# factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
# factionFileNames = [
#     factionDefFolder + 'Faction_FirstOrder'
# ]

# createFactionPatch(
#     otherModsPath+'\\' +
#     destinationFolderNameFaction_SWFirstOrder+'\\'+destinationPathVersion,
#     factionFileNames,
#     modIDFaction_SWFirstOrder,
#     species,
#     pawnKindDefNamesArray
# )
# # ================================VEF Settlers========================================
# # ================================VEF Settlers========================================
# # ================================VEF Settlers========================================

factionDefFolder = '\\1.4\\Defs\\FactionDefs\\'
factionFileNames = [
    factionDefFolder+'Factions_Settlers'
]


createFactionPatch(
    otherModsPath+'\\' +
    destinationFolderNameFaction_VESettlers+'\\'+destinationPathVersion,
    factionFileNames,
    modIDFaction_VESettlers,
    species,
    pawnKindDefNamesArray
)
