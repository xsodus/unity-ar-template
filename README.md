# The AR template project for Unity.
I've created this project because AR origin was deprecated in Unity 2022.3.22f1. There is no full walkthrough guide line to setup so this project will help you to speed up initial the AR project.

## How to use

1. Open this project in the Unity 2022.3.22f1.
2. Go to `File` -> `Build Settings`.
3. Select `iOS` platform
4. Click on `Player Settings` buttom
5. Go to `Other Settings` section then change `Bundle Identifier` to your app name.
6. Close the `Project Settings` window
7. Then click `Build And Run` button
8. The XCODE may request you to login with Apple Developer account for running on the physical device. You need to login and install certificates by this guide: https://developer.apple.com/documentation/xcode/sharing-your-teams-signing-certificates
9. You need to enable `Automatically manage signing` and put the correct team name for siging the app. You can check more detail here https://developer.apple.com/documentation/xcode/adding-capabilities-to-your-app/
10. Select the device target to your iPhone (physical iphone)
11. Run the app.
12. When the app is opened. Point your front camera to your face.
13. You should see the green mask on your face.

## TODO
- [x] iOS setup
- [ ] Android setup


