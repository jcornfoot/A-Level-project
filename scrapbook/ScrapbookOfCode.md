# Scrapbook of code 

While setting up my git & github repos I encountered an issue, I wanted to keep all of my code on git for version control and overall management of bugs and features.
To do this I needed to add my code to my git repo, the issue was that my code also needed to be within my Unity project or else it would not compile.
To solve this I figured that I could use a symbolic link to connect a file in my repo to the code in my project, this was an issue however as git did not correctly track the files I had linked.
Instead I tried out using a Hardlink, another pointer to the files inode. This fixed the issue and now every change to this linked file is tracked by git.
The only issue with this solution is that I cannot hardlink to directories so I will have to create a new link to each file I add.
