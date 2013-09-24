!! Important !!
The proxy context code is used by SiteContext and DebugContext.  It cannot and should not use any other items from the container within itself, as it needs to operate at the highest level.
In other words do not try to use ISiteContext or IDebugContext from within the IProxyContext provider.
Do not use member variables to save state in HeaderValues classes.  The Check method is called per different requests.