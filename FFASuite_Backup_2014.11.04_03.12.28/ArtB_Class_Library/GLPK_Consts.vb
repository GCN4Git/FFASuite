Module GLPK_Consts
    Public Const GLP_MIN = 1  ' minimization 
    Public Const GLP_MAX = 2  ' maximization 

    ' kind of structural variable: 
    Public Const GLP_CV = 1  ' continuous variable 
    Public Const GLP_IV = 2  ' integer variable 
    Public Const GLP_BV = 3  ' binary variable 

    ' type of auxiliary/structural variable: 
    Public Const GLP_FR = 1  ' free variable 
    Public Const GLP_LO = 2  ' variable with lower bound 
    Public Const GLP_UP = 3  ' variable with upper bound 
    Public Const GLP_DB = 4  ' double-bounded variable 
    Public Const GLP_FX = 5  ' fixed variable 

    ' status of auxiliary/structural variable: 
    Public Const GLP_BS = 1  ' basic variable 
    Public Const GLP_NL = 2  ' non-basic variable on lower bound 
    Public Const GLP_NU = 3  ' non-basic variable on upper bound 
    Public Const GLP_NF = 4  ' non-basic free variable 
    Public Const GLP_NS = 5  ' non-basic fixed variable 

    ' scaling options: 
    Public Const GLP_SF_GM = 1 ' perform geometric mean scaling 
    Public Const GLP_SF_EQ = 16 ' perform equilibration scaling 
    Public Const GLP_SF_2N = 32  ' round scale factors to power of two 
    Public Const GLP_SF_SKIP = 64 ' skip if problem is well scaled 
    Public Const GLP_SF_AUTO = 128  ' choose scaling options automatically 

    ' solution indicator: 
    Public Const GLP_SOL = 1  ' basic solution 
    Public Const GLP_IPT = 2  ' interior-point solution 
    Public Const GLP_MIP = 3  ' mixed integer solution 

    ' solution status: 
    Public Const GLP_UNDEF = 1  ' solution is undefined 
    Public Const GLP_FEAS = 2  ' solution is feasible 
    Public Const GLP_INFEAS = 3  ' solution is infeasible 
    Public Const GLP_NOFEAS = 4  ' no feasible solution exists 
    Public Const GLP_OPT = 5  ' solution is optimal 
    Public Const GLP_UNBND = 6  ' solution is unbounded 

End Module
