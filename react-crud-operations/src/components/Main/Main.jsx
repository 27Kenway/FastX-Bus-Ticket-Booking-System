import { Link } from 'react-router-dom';
import './Main.css'
import hotel from './hotel.jpeg'

//import video1 from './public/images/video1.mp4';
import video1 from './video1.mp4';


function Main(){
    return(
        <div>
<div className="banner">
            {/* <video autoPlay loop muted playsInline>
                    <source src={video1} type="video/mp4"/>
                </video> */}
          <img src={hotel} alt="Hotel" className="banner-image" />
            <div className="content">
                <h1>Make Your Trip Much Better with FastX Bus Services</h1>
                <div>
                    <Link to="/fromandto">
                        <button type="button" className='buttonStyle1'>Book Your Bus</button>
                        </Link>

                    <Link to="/registerbusoperator">
                        <button type="button" className='buttonStyle2'>Bus Operators, Register Here</button>
                        </Link>
                        
                </div>
            </div>
        </div>

        {/* <div className="about-section">
            <div className="about-content">
                <h2>About FastX Bus Services</h2>
                <p>
                    Welcome to FastX Bus Services! We strive to provide a seamless and enjoyable bus booking experience. 
                    Explore our wide range of destinations and book your bus tickets with ease. Bus operators can also 
                    register with us to showcase their services to a broader audience. Join us on the journey of hassle-free
                    and comfortable bus travel.
                </p>
            </div>
        </div>

        <div className="scroll-text">
            <p>Scroll down to learn more about our services and offerings.</p>
            </div> */}
</div>
    )
}
export default Main;