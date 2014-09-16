[CmdletBinding(SupportsShouldProcess)]
Param()

# This is a powershell array, add more configs to the array to execute multiple test projects

@(
  @{
    UnitTestProjectName = 'Atlantis.Framework.DCCGetDomainInfoByID.Tests'
    UnitTestToolType = 'mstest'
    UnitTestRelativePath = 'Atlantis.Framework.DCCGetDomainInfoByID.Tests\bin\release'
    UnitTestTestsContainer = 'Atlantis.Framework.DCCGetDomainInfoByID.Tests.dll'
    UnitTestUserSuppliedArgs = @{
      ## optional for mstest ##
      #testmetadata = see mstest docs
      #testlist = see mstest docs
      #category = see mstest docs
      #test = see mstest docs
      #noisolation = `$null
      #testsettings = see mstest docs
      #detail = see mstest docs
    }
    UnitTestCodeCoverageToolType = 'opencover'
    UnitTestCodeCoverageUserSuppliedArgs = @{
      ## optional for opencover ##
      #coverbytest = see opencover docs
      excludebyattribute = '(*.ExcludeFromCodeCoverageAttribute)'
      #excludebyfile = see opencover docs
      filter = '"+[Atlantis.Framework.DCCGetDomainInfoByID*]* -[Moq*]* -[Atlantis.Framework.DCCGetDomainInfoByID.Tests]* -[Atlantis.Framework.Testing*]*"'
      #log = see opencover docs
      #oldstyle = `$null
      #returntargetcode = see opencover docs
      #service = see opencover docs
      #showunvisited = `$null
      #skipautoprops = `$null
      #targetdir = see opencover docs
      #threshold = see opencover docs    
    }
    #UnitTestCodeCoverageVsInstrFileRegex = Optional value.  regex that describes with files to include from `$UnitTestRelativePath in the code coverage analysis
    #UnitTestCodeCoverageVsInstrRecurseSubDirectories = Optional value.  whether to recursively search directories and apply UnitTestCodeCoverageVsInstrFileRegex
  }
)