This document describes how you can compile, run and debug this module.

# About this module
TBD

# Requirements
- Visual Studio 2022
- Visual Studio Code
- DotNetNuke Development Environment
- GIT
- Node.JS

# Build
For building you need both use Visual Studio and Visual Studio code.

## Compile the assets
- Open the project folder with Visual Studio code
- Run NPM install

```
npm i
```

- Run gulp build via NodeJS:

```
npx gulp build
```

## Compile and build the project
- Open the project with Visual Studio
- Build the project in DEBUG configuration
- Build the project in RELEASE configuration

# Install and Run
- Find the DNN module package in the install directory
- Install the "Install" package

# Changes made to the template
In order to have the NPM tools work, some minor changes have been applied to the template.

## ModulePackage.targets
The "node_modules" folder was excluded from the end-user include package. The folder however _added_ to the source package.