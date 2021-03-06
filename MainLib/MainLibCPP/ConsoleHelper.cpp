#include "stdafx.h"
#include "ConsoleHelper.h"


ConsoleHelper::ConsoleHelper()
{
}


ConsoleHelper::~ConsoleHelper()
{
}

BOOL ConsoleHelper::PrintStrings(HANDLE hOut, ...)
{
	DWORD msgLen, count;
	LPCTSTR pMsg;
	va_list pMsgList;

	va_start(pMsgList, hOut);

	while ((pMsg = va_arg(pMsgList, LPCTSTR)) != NULL) {
		msgLen = _tcslen(pMsg);

		if (!WriteConsole(hOut, pMsg, msgLen, &count, NULL)
			&& !WriteFile(hOut, pMsg, msgLen*sizeof(TCHAR), &count, NULL)
			) {
			va_end(pMsgList);
			return FALSE;
		}
	}
	va_end(pMsgList);
	return TRUE;
}

BOOL ConsoleHelper::PrintMsg(LPCTSTR pMsg)
{
	HANDLE hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
	BOOL result = PrintStrings(hStdOut, pMsg, NULL);
	CloseHandle(hStdOut);
	return result;
}

void ConsoleHelper::print(string s)
{
	StringHelper stringHelper;
	BOOL result = PrintMsg(stringHelper.s2ws(s));

	if (result) {
		cout << "Fatal Write Error" << GetLastError() << endl;
	}
}

void ConsoleHelper::printLine(string s)
{
	StringHelper stringHelper;
	
	BOOL result = PrintMsg(stringHelper.s2ws(s.append("\r\n")));

	if (result) {
		cout << "Fatal Write Error" << GetLastError() << endl;
	}
}

void ConsoleHelper::ReportError(LPCTSTR userMessage, DWORD exitCode, BOOL printErrorMessage)
{
	DWORD eMsgLen, errNum = GetLastError();

	LPTSTR lpvSysMsg;

	_ftprintf(stderr, _T("%s\n"), userMessage);

	if (printErrorMessage) {
		eMsgLen = FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER |
			FORMAT_MESSAGE_FROM_SYSTEM,
			NULL,
			errNum,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			(LPTSTR)& lpvSysMsg, 0, NULL);

		if (eMsgLen > 0) {
			_ftprintf(stderr, _T("%s\n"), lpvSysMsg);
		}
		else {
			_ftprintf(stderr, _T("Last Error Number; %d.\n"), errNum);
		}

		if (lpvSysMsg != NULL) LocalFree(lpvSysMsg);
	}

	if (exitCode > 0)
		ExitProcess(exitCode);

	return;
}
