#------------------------------------------------------------------------------
# IronPython Calculator autorun file
# Written by: webmaster442
#------------------------------------------------------------------------------

#------------------------------------------------------------------------------
# .net container imports
#------------------------------------------------------------------------------
from System.Collections.Generic import Stack
from System.Collections.Generic import Queue
from System.Collections.Generic import List
from System.Collections.Generic import Dictionary

#------------------------------------------------------------------------------
# .net Class imports
#------------------------------------------------------------------------------
from System import BitConverter

#------------------------------------------------------------------------------
# Static class type
#------------------------------------------------------------------------------
class StaticClass:
	def __init__(self, anycallable):
		self.__call__ = anycallable

#------------------------------------------------------------------------------
# .net import list
#------------------------------------------------------------------------------

NetImports = ["Stack", "Queue", "List", "Dictionary", "BitConverter"]

def dotnetimports():
	print "--------------------------------------------------------------------"
	print ".net imported types"
	print "--------------------------------------------------------------------"
	for i in NetImports:
		print i
