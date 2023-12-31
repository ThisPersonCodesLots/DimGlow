## DimGlow
- The application features a user-friendly interface with a trackbar for adjusting the transparency level of the overlay.

- DimGlow provides a sleek and efficient solution for creating a comfortable viewing experience in low-light environments.

## Usage ##
- The range of the trackbar is from 0 to 95. Also I include a restriction to the trackbar so that the user can't use the value of +100 (It will be too dark).
- The mouse and Window Manager are still visible because of Win32 Registers.
- If any related HTML5 videos are still visible as normal bright it's because of some Video Enhancement Software (AMD Radeon, Intel, etc). Make sure you check them!
- In case you slide the trackbar all the way to the right (maximum value) and the screen become to dark, you can increase the brightness of the screen psychically.

## **WARNING**:
- **WARNING**: This application triggers **_Windows Defender SmartScreen_**. This is because the application is **NOT SIGNED**. To run the application, click on **"More info"** and then **"Run anyway"**. The application is open-source and the source code can be viewed in the repository.

- If you don't trust me, you can build the application yourself using Visual Studio.

-  The executable file will be located in the bin folder of the project directory. Just move the entire /src files in the main directory and run *DimGlow.sl*

### TODO:
- [ ] Solve the issue with the application triggering Windows Defender SmartScreen.
- [x] Solve the issue with "Dark Mode" checkbox to remain saved when the application is closed.
- [x] Show battery status.
- [x] Multi-monitor support.
- [ ] Multi-monitor separate functionality.

### Image
	- Normal mode
![alt-text](https://i.imgur.com/s7w9dZV.png)

	- Transparent mode
![alt-text](https://i.imgur.com/WDo0guB.png)


### Behind the scenes
- This project was built using Visual Studio 2022. To run the application, open the solution file in Visual Studio and build the project.

- It's used primarily for reducing eye strain when using a laptop screen in dark environments (e.g. in bed, on a plane, etc.).

- Reducing the amount of light emitted from the screen to improve battery life.

- Learn how to use the Win32 API in C# and to get rid of the annoying blue light filter that comes with Windows 10/11.
