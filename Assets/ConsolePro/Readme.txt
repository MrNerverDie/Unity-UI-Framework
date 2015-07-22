Editor Console Pro 2.0 is a powerful replacement for Unity's editor console.

It is accessed in the Window menu under Console Pro, or by pressing Command + \

# Panel buttons
The top right buttons allow you to hide/show various things:
Color toggles colorized entries.  When this is on the entire line of a log will have the same color as it's filter.  For example, Errors will be red.
File toggles the file name that the Debug.Log was called from in a column.
Class toggles the class and method name that the Debug.Log was called from in a column.
Time toggles the time stamp for the Debug.Log.
Object toggles the name of the object calling the log when used with Debug.Log("Log", GameObject);
Stack toggles the stack trace panel.
Src toggles source code view in the stack trace panel.  This lets you see the surrounding source code for every function call in the stack trace.  Clicking on any line in the source view will let you jump directly to it.

# Columns
You can reorder or completely disable columns in the preferences window under Columns.  When a column is disabled from preferences, it no longer has a button on the top to show/hide it.

# Source view
When source view is toggled on (via the Src button on the top right), each stack entry with a file associated with it will automatically show you the surrounding source code.  When this is disabled, you can still view the source code of a stack entry by clicking "Source..." next to the stack entry.

# Ignore Classes
You can ignore classes so they do not show up in the stack, and double clicking the log will go to the first unignored class.  This is very useful for helper functions such as:
void MyLog(string inString)
{
	if(debugMode)
	{
		Debug.Log(inString);
	}
}
Once you ignore this log, it's as if you called Unity Debug.Log directly.
You ignore them by clicking on Ignore next to a stack entry.  To remove/manage ignores, look under Ignore Classes in the Preferences.

# Filtering by search:
Type a search string in the top right <filter> field to only show entries containing the search text.

# Using Regular Expressions:
Type @ before a search string in the search field or in custom filter strings to search using a regex.  For example:
@[A-Za-z]

# Custom filters:
Custom filters allow you to add more log types to the toolbar next to Errors, Warnings, Logs and to give them a custom color.  You can filter them by type and string.
Create a filter by:
Press the Prefs button to bring up preferences.
Press New Filter.
Here you can name your filter, select the log type (error, warning, log), pick what to filter from (file name, class/method containing the debug.log call, object name, or log text), and a color to identify them with.

# Jumping to stack entries
Simply click on any line in the stack panel to jump to that line in the source code.  Double clicking a log entry jumps to the top stack entry as normal.

# Copying and Exporting
Right click an entry and press Copy Stack to put a copy of the current stack into the clipboard, or Copy Log to put a copy of the current log into the clipboard.
Press Export and pick a file name to export a text file of the entire log.  You can use this to compare against different plays of your game.

# Support
Please EMail all support and feature requests to Support@Flyingworm.com