set HtmlCleanerCmd=java -Dfile.encoding=utf-8 -jar ../htmlcleaner-2.2.jar
set DestDir=Clean

cd Data
for /f %%f in ('dir /b *.htm') do %HtmlCleanerCmd% src=%%f dest=%DestDir%\%%f
