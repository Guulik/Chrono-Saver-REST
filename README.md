Chrono Saver is save manager for your game saves. Chrono - your save, your entire adventure.
The current version is the universal version of Chrono Saver WPF that can work with any of your games, not just The Legend of Zelda Breath of The Wild as it was before. The application itself is a RestAPI and a small test frontend

# Deployement
First you should clone the repository via

`git clone https://github.com/Guulik/Chrono-Saver-REST.git`

Then download and start the backend container from docker to do this, enter the following command

`docker pull guulik/chrono_saver_api`

Or use docker desktop to download the guulik/chrono_saver_api image.

Start the container on port 5194
you can use this command 

`docker run -p 5194:5194 guulik/chrono_saver_api`

That's it, the backend is up and running, you can run the application!

The Frontend.exe executable file is located at this path
`{your_local_path_to_repository}\Frontend\bin\Release\net8.0-windows/Frontend.exe`

I think the interface is intuitive. If you have any questions, email me at gulixandr@gmail.com with the subject line "Chrono Saver Q".

Use it with pleasure! 


