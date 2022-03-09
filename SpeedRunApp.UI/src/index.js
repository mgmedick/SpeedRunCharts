import ComponentLoader from './components/component-loader.js'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'vue-next-select/dist/index.min.css'

import { library, dom } from '@fortawesome/fontawesome-svg-core'
import { faCertificate, faPercentage, faAward, faCubes, faStar, faFire, faGamepad, faSpinner, 
         faSearch, faComment, faChevronDown, faChevronRight, faHourglassEnd, faCircleCheck, 
         faExclamationCircle, faPlayCircle, faTrophy } from '@fortawesome/free-solid-svg-icons'

library.add(faCertificate, faPercentage, faAward, faCubes, faStar, faFire, faGamepad, faSpinner, 
            faSearch, faComment, faChevronDown, faChevronRight, faHourglassEnd, faCircleCheck,
            faExclamationCircle, faPlayCircle, faTrophy);
dom.watch();

import './fonts/TwitchGlitchPurple.svg';
import './fonts/Twitter_Logo_WhiteOnBlue.svg';
import './fonts/youtube_social_square_red.svg';
import './fonts/bar-chart.svg';
import './fonts/pie-chart.svg';
import './fonts/SpaceInvader.svg';

ComponentLoader.loadComponents();


