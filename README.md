Simple code example that shows Windows Azure Diagnostic (WAD) data collection for Trace statements and IIS logs.
* Interesting code is in WebRole.cs
* Make sure there is a valid storage account attached
* Normally, it is a good idea for the WAD storage account to be a different account from business data; this is for performance/scalability reasons as well as operational simplicity (different groups typically will want access to the different systems)
	