<#
	Removes node_moddules directories from the specified directory and all subdirectories.
#>
function Remove-FoldersByName {
	Param
	(
		[Parameter(Mandatory = $False, ValueFromPipeline = $True, ValueFromPipelinebyPropertyName = $True, Position = 0)]
		[PSDefaultValue(Help = 'Path to search. Default=CWD')]
		[string[]]$Path = '.',
		
		[Parameter(Mandatory = $False, Position = 1)]
		[PSDefaultValue(Help = 'Folder name to search. Default=Node_Modules')]
		[string]$FName = 'Node_Modules'
	)

	Write-Output $FName
	do {
		try {
			$dirs = Get-ChildItem $Path -directory -recurse | 
			Where-Object { (Get-ChildItem $_.name) }  -eq $FName | 
			Select-Object -expandproperty FullName
			$dirs | Foreach-Object { Remove-Item $_ }
		}
		catch {
			$valid = Test-Path -Path $_ -IsValid
			if ($valid -eq $false) {
				Write-Host "Invalid Path: $_"
			}
			else {
				Write-Output $_
			}
		}
	} while ($dirs.count -gt 0)
}

<#
	Removes Empty Directories
#>
function Remove-EmptyDirs {
	Param
	(
		[Parameter(Mandatory = $False, ValueFromPipeline = $True, ValueFromPipelinebyPropertyName = $True)]
		[PSDefaultValue(Help = 'Path to search.')]
		[string[]]$Path = '.'
	)

	do {
		$dirs = Get-ChildItem $Path -directory -recurse | 
		Where-Object { (Get-ChildItem $_.fullName -force).count -eq 0 }  | 
		Select-Object -expandproperty FullName

		try {
			$dirs | Foreach-Object { Remove-Item $_ }
		}    
		catch {
				Write-Output $PSItem.Exception.Message
		}
	} while ($dirs.count -gt 0)
}



<#
	Moves files that LR failed to import to the specified directory and all subdirectories.
#>
function Move-LRFailed {
	Param
	(	
		[Parameter(Mandatory = $True, Position = 0)]
		[PSDefaultValue(Help = 'Failure file to get ')]
		[string]$FName,
		
		[Parameter(Mandatory = $False, ValueFromPipeline = $False, ValueFromPipelinebyPropertyName = $True, Position = 1)]
		[PSDefaultValue(Help = 'Path to search. Default=CWD')]
		[string[]]$Path = '.'
	)

		New-Item -ItemType Directory -Force -Path $Path
		$outDir = Get-Item $Path
		$count = 0;
		foreach($line in Get-Content $FName) {
		if($line)
		{
			$count++;
			$trim = Get-Item $line.Trim()
			$exists = Test-Path $trim 
			if($exists)
			{
				$outputFileName = Split-Path $trim -leaf
				$outputFile = Join-Path -Path  $outDir -ChildPath $outputFileName
			
				Write-Host "$count Moving $line to $outputFile"
				Copy-Item -Path $trim -Destination $outputFile
			}
		}
	}
}