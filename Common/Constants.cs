using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Common
{
    public static class Constants
    {
        public const string APILogID = "APILogID";
        public const string DateTimeForamt = "yyyyMMddHHmmss";
        public const string TempFolder = "TempFolder";
        public const string EmployeeDocs = "EmployeeDocs";
        public const string DoctorDocs = "DoctorDocs";
        public const string ChemistDocs = "ChemistDocs";
        public const string PopupDocs = "PopupDocs";
        public const string ExpenseDocs = "ExpenseDocs";
        public const string ExpenseMasterDocs = "ExpenseMasterDocs";
        public const string lmsAuthToken = "DBOqhGeXrbqxgqgOK6x3nIQuOGjPHrMWw9fuu5xJ";
        public const string IssueTrackerDocs = "IssueTrackerDocs";
        public const string EDApprovalDocs = "EDApprovalDocs";
    }


    public enum StatusCodes : int
    {
        //GlobalCodes
        Success = 800,
        UnAuthorised = 801,
        InvalidToken = 802,
        TokenExpired = 804,
        InvalidCredential = 805,
        ServerError = 806,
        InvalidInput = 807,
        NoDataFound = 808,
        SomethingWentWrong = 809,
        ActionPerformedSuccessfully = 810,
        
        //CommonAPIcodes
        RecordSavedSuccessfully = 901,
        RecordDeletedSuccessfully = 902,
        RecordFetchedSuccessfully = 903,
        AnErrorhasoccured = 904,
        FileUploadedSuccessfully = 905,
        FileUploadError = 906,
        PasswordExpired=907,
        FileDeletedSuccessfully = 908,
        FileDeleteError = 909,
        RecordUpdatedSuccessfully = 910,
        InvalidUsernameOrPassword = 1001,
        Youhaveusemaximumattempts, yourAccountHasbeenlocked = 1002,
        YourAccounthasbeenlocked = 1003,
        LoginSuccesffuly = 1004,
        EmployeedoesnotexistwiththisEmployeeNo = 1005,
        EmployeeExist = 1006,
        Otherdetaisarematch = 1007,
        DetailsdoesnotmatchwiththisEmployeeNo = 1008,
        PasswordResetSuccesfully = 1009,
        CanNotSetYourPreviousPasswordasCurrentPassword = 1010,
        LogOutSuccessfully = 1011,
        LogOutNOTSuccessfully = 1012,
        LoginForgotLogSaved = 1013,
        LoginForgotLogNOTSaved = 1014,
        Recordalreadyexistwithsametype = 1015,
        Menunamewithparentmenualreadyexists = 1016,
        // = 1017,
        EmployeeInactive = 1018,
        EmployeeUnLockedSuccessfully = 1019,
        EmployeeLockedSuccessfully = 1020,
        AttechedFileisEmpty = 1021,
        NotSupportfileextension = 1022,
        VacantEmployeeInsertedSuccessfully = 1023,
        VacantEmployeeNOTInsertedSuccessfully = 1024,

        UniquePositionCodeAlreadyExists = 1025,
        EmployeeEmailAlreadyExists = 1026,
        EmpnoAlreadyExists = 1027,

        DocRegNoExists = 1028,
        DocMobNoExists = 1029,
        DocPanNoExists = 1030,
        DocBDExists = 1031,
        DocEmailExist = 1032,

        ChemistAdvaitNoExists = 1033,
        ChemistLicenseNoExists = 1034,
        ChemistTinnoExists = 1035,
        StockiestNameExists = 1036,
        PetNameExists = 1037,
        InstitutionNameExists = 1038,
        AlreadyExists = 1039,
        InvalidUniquePositionCode = 1040,
        NotMarkedAsDraft = 1041,
        MaxLegendsForVIP = 1042,
        MaxLegendsForA = 1043,
        MaxLegendsForB = 1044,
        InvalidFile = 1045,
        AlreadyVacant = 1046,
        NotFSEmployee = 1047,
        InvalidEmpNo = 1048,
        EmpAlreadyinVacantProcess = 1049,
        SuccessVacantProcess = 1050,
        UnfreezeRequestAlreadyInProcess = 1051,
        ResignAndTransAllow = 1053,
        AttechedFileFormateIssue = 1052,
        DoctorDeactivate = 1054,
        DoctorActivated = 1055,
        ChemistDeactivated = 1056,
        ChemistActivated = 1057,
        DivisionNameExists = 1060,
        ParaWithEntityCodeAndNarrationExists = 1061,
        ParaWithEntityCodeAndValueExists = 1062,
        RegionNameExists = 1063,
        DistrictNameExists = 1064,
        DivisionCodeExists = 1065,

        InvalidResetPasswordLink = 1066,
        ValidResetPasswordLink= 1067,
        InValidEmailAddress = 1068,
        SampleRequestExists = 1069,
        CallTypeAlreadyExists = 1070,
        CallSubTypeAlreadyExists = 1071,
        DoctorExpirySet = 1072,
        Doctortransfered = 1073,
        DoctorNotDuplicate=1074,
        HolidayWorkRequestExists = 1075,
        CantdoFutureDateValidation = 1076,
        MonthPlanNotApproved = 1077,
        NeedToReportAllPlanedDoctors = 1078,
        CantReportForThisDay = 1079,
        MinimumChemistrequired = 1080,
        MinimumStockistrequired = 1081,
        HolidayValidation =1082,
        SecondarySalesUpationDateCloased=1083,
        BIBONotUploaded = 1084,
        AIOCDStockiestDateNotMatched = 1085,
        SecondarySalesSubmittedbyOtherMR = 1086,
        NeedReportingInChronological = 1087,
        InvalidStockiestSAPCodeinUpload = 1092,
        cannotadddoctorsonleave = 1093,
        LeaveBalance = 1091,
        LeaveMonthlyLimit = 1094,
        SandwichLeave = 1095,
        ApplyOnDayLeave = 1096,
        ApplyforLeaveRequest = 1097,
        LeaveCancelRequest = 1098,
        AllreadyPlannedDoctor = 1099,
        AreaNameExists = 1100,
        SFCExistsWithSameCity = 1101,
        SampleAndPETConfirmation = 1102,
        FinalReportSubmitted = 1103,
        RailwayFareExists = 1104,
        CallTypeMasterEmployeeNameExists = 1105,
        OlderLeave = 1106,
        HotelFareExists = 1107,
        ExpenseLockDateAllreadyApply = 1108,
        ExpenseAllreadySubmitted = 1109,
        SVLDoctorUpdateRequestExists = 1110,
        Allreadysubmitdailyreport = 1111,
        QtyCantgraterthanbalance = 1112,
        MonthlyPlanNeedToApproved = 1113,
        ReportingSlotUsed = 1114,
        MoleculeNameExists = 1115,
        CompetitorCompanyNameExists = 1116,
        CompetitorBrandNameExists = 1117,
        VendorNameExists = 1118,
        TravelTypeNameExists=1119,
        SAPCodeExists=1120,
        ActivityNameExists = 1121,
        ActivityNoOfDoctorsNotMatched = 1122,
        ActivityNotMarkedAsDraft = 1123,
        NoMoleculeConfigfound = 1124,
        DataExists = 1125,
        VisitingLocationRequired = 1126,
        RegionIDExists=1127,
        ActivityEnddateClose= 1128,
        InvalidPetSAPCode = 1129,
        RegionGroupNameExists=1130,
        DoctorSelectedForCampaingActivity = 1131,
        AdminActivityNameExists = 1132,
        ActivityTypeNameExists=1133,
        EdcfActivityNameExists=1134,
        EdcfExpenseNameExists=1135,
        EdcfSPMMaxReqPerMonthReached = 1136,
        LeavePolicyNameExists=1137,
        LeaveTypeNameExists = 1138,
        LeaveGroupPolicyNameExists = 1139,
        LeaveGroupNameExits=1140,
        DivisionGroupNameExits =1141,
        TitleExits=1142,
        HolidayExits=1143,
        SSGAppliationCreated=1144,
        ApplicationBackToSA=1145,
        FinanaceYearExits = 1146, 
        ActivityTypeCodeExists = 1147,
        EdcfDuplicateBillData = 1148,
        InvalidEmployeesForAdvanceEdcf = 1149


        //APIresponsecodes
    }

    public enum ResponseCodes : int
    {
        //Common API codes


        //API response codes
    }
}
