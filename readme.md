<div id="top"></div>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://avatars.githubusercontent.com/u/109361408?s=400&u=e1530a760f1b11646a0cd9d13e9776a0c6bdf964&v=4">
    <img src="https://avatars.githubusercontent.com/u/109361408?s=400&u=e1530a760f1b11646a0cd9d13e9776a0c6bdf964&v=4" alt="Logo" width="200" height="200">
  </a>

<h1 align="center">Scaffold Library</h1>

  <p align="center">
    A toolkit manager! have all your tools at your disposal at any time on your unity projects! 
    <br />
    Or use mine!
    <br />    <br />
    Explore all of the Scaffolds modules to kickstart your project.
    <br />
    <a href="https://github.com/github_username/repo_name/issues">Report Bug</a>
    ·
    <a href="https://github.com/github_username/repo_name/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)
Scaffold started as a way to keep all my codebase in a single place and share it accross projects without the need for actually looking for links, manually exporting packages or worrying about dependencies.

It partially mimics the Package Manager, but allowing me to handle git-dependencies! It's also easy to change the entry-points and use it for your own repositories as well.

Scaffold also contains a Module-Builder that allows me to delegate and encapsulate the building and updating of new modules to outside of the main scaffold package. This way any module can be freely edited without worrying for the overall library - and without having to memorize any specific structure the Scaffold Library requires.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

Using Scaffold's library is really simple, you just have to install it and you will have access to the Scaffold's launcher window, listing all the avaiable modules to you! no further setup is required other than installing the project.

To use the Module-Builder or swapping the library for your own code base should take you a few more steps, but nothing more than a few button clicks!

### Installation

You can install Scaffold by either downloading the package or adding the package URL to the package manager or manifest! <br/><b>Note: Don't use this git default's URL as the package - it wont work, the package is nested in a internal folder </b>

* <b>Download Unity Package:</b>

    ...pending...

<b>OR</b>

* <b>Add path to Package Manager:</b>
   ```sh
   https://github.com/MgCohen/Scaffold.git?path=/Assets/Scaffold/Launcher
   ```

  And that's it! no further setup is required.


### Installing the Module-Builder

To install the Module-Builder you can either add it manually to the package-manager or just click on the unity tool bar!
<br />
note: to use the Builder on the main scaffold project, check <a href="https://github.com/github_username/repo_name/issues">Contributing</a>

* <b> Toolbar:</b>

    [Scaffold -> Builder -> Install Builder]
    
    ![installing Builder](https://imgur.com/HPyc3IJ.png)
 
<b>OR</b>

* <b>Add path to Package Manager: </b>

   ```sh
   https://github.com/MgCohen/Scaffold.git?path=/Assets/Scaffold/Builder
   ```
   
   
### Swapping the library for your own

Swapping the library modules is as easy as swapping the url for retrieval! Scaffold uses a simple HTTP request to get the list in a json format.
To swap the url you just have to fork this repository, and edit the url on the Library Object. 

  ![Swapping Library](https://imgur.com/Tx74Eux.png)

To swap the url directly in the package, you will need to either make the scriptable object editable or do it through code! the <b>Libray Url</b> property is public.


<!-- USAGE EXAMPLES -->
## Usage

* <b> Launcher: </b>
    Using is as simple as tapping Ctrl+shift+L and choosing which modules to install! Scaffold handles module installing and use custom scripting defines to guarantee your project dependencies are correct.

    ...image 1....

    ![Library](https://imgur.com/HCPamRY.png)

    Click on the gear icon at the side of each module to install/update/uninstall - it will handle everything for you.
    
* <b> Builder: </b>

    ...builder guide pending....

<!-- ROADMAP -->
## Roadmap

- [X] Launch v1
- [ ] Stability test and bug-fixing
- [ ] Add auto-updater for Launcher and Builder
- [ ] Refactor module library repository to accomodate contributers
- [ ] Add Config tab to facilitate url customizations
- [ ] Handle git-dependencies outside of the library modules
- [ ] Version checking for dependencies
- [ ] Sending local package updates directly to main repository

I also have a bunch of little tools that i plan to refactor and bring into the library!
Although not in any particular order, you can see that some of the modules in the roadmap are necessary for subsequent modules!

- [X] Events
- [ ] Screens
- [ ] Noise Utility
- [ ] Search & Algorithms
- [ ] PathFinder
- [ ] Grid system
- [ ] Isometric Toolkit
- [ ] Hexagon Toolkit
- [ ] Draggable system
- [ ] Card System
- [ ] Saving System
- [ ] Simple Dependency Injection
- [ ] Scenes Crawler & Finder
- [ ] CSV Importer
- [ ] Localization
- [ ] Cached Variables
- [ ] Http jumpstart kit
- [ ] Aws & firebase jumpstart kits


<!-- CONTRIBUTING -->
## Contributing

This project started without any planning - and that can be seen on a few places where a little refactoring may be needed! no problem, all help is appreciated!

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Adding new modules to the library

If you want to actually share a new Module to the library - i'll be probably refactoring the way i send/fetch modules soon to accomodate a more open-source style.

As of right now - you can still try to build your module with the Module-Builder, but you will need a credential file that would send the package data directly to the library DB.


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<!-- CONTACT -->
## Contact

Your Name - [@twitter_handle](https://twitter.com/twitter_handle) - email@email_client.com

Project Link: [https://github.com/github_username/repo_name](https://github.com/github_username/repo_name)


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com 
