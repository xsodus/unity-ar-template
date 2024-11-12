# AR Template Project for Unity 6.0

This project was created to address the deprecation of the AR Origin component and functions in Unity 6.0. It aims to streamline the initial setup process for AR projects by providing a pre-configured foundation.

## Dependencies

This project relies on the following dependencies:

- **Unity AR Foundation**: Provides a framework for building AR applications.
- **ARKit XR Plugin**: Required for AR functionality on iOS devices.
- **ARCore XR Plugin**: Required for AR functionality on Android devices.
- **XR Plug-in Management**: Manages the loading of XR plugins and provides a unified interface for configuring and managing XR settings across different platforms.
- **XR Simulation Environments**: Allows testing and development of AR applications within a simulated environment, reducing the need for physical devices during the initial stages of development.

## Instructions for iOS Platform

1.Open the project in Unity version 6.0.

2.Navigate to "File" > "Build Settings."

3.Select the "iOS" platform.

4.Click the "Player Settings" button.

5.Under "Other Settings," change the "Bundle Identifier" to your desired app name.

6.Exit the "Project Settings" window.

7.Click the "Build And Run" button.

8.Xcode might prompt you to log in with your Apple Developer account for physical device deployment. Follow the guide at https://developer.apple.com/help/account/get-started/sign-in-to-your-developer-account/ to log in and install the necessary certificates.

9.Enable "Automatically manage signing" and ensure the correct team name is selected for app signing. Refer to https://developer.apple.com/documentation/xcode/adding-capabilities-to-your-app/ for more details.

10.Choose your target iPhone (physical device) from the available options.

11.Run the app.

## Instructions for Android Platform

1. Enable developer mode on your Phone then connect to your laptop or pc

2. You should see the device by running `adb devices` on the terminal window.

3. Open project with Unity.

4. Navigate to "File" > "Build Settings."

5. Press on "Build and run" button to run the app on your phone

Notes: Do not enable Occulus provider in `XR Plug-in Management` because the android phone doesn't support.

## Testing

Camera Focus: Upon app launch, direct the front camera towards your face.

Expected Result: A green mask should appear on your face, signifying successful implementation.

## Development Status

- [x] iOS Setup
- [x] Android Setup
- [ ] Occulus Quest (WIP)
- [x] AR Face Tracking Test Scene
- [x] AR Image Tracking Test Scene
- [x] AR Plan Detection Test Scene
