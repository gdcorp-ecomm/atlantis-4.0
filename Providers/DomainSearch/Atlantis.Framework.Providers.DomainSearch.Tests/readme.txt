!! Important !!
The proxy context code is used by SiteContext.  It cannot and should not use any other items from the container within itself, as it needs to operate at the highest level.
It can use DebugContext, and that is about it.
In other words do not try to use ISiteContext or other providers from within the IProxyContext provider.
Do not use member variables to save state in HeaderValues classes.  The Check method is called per different requests.