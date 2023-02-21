import "./AboutUs.css";
import Header from "../Header/Header";
import db from "./firebase";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { BsGithub } from "react-icons/bs";
import { MdEmail } from "react-icons/md";
import {VscClose} from "react-icons/vsc";

function AboutUs(props) {
  const [email, setEmail] = useState("");
  const [name, setName] = useState("");
  const [message, setMessage] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();

    db.collection("contacts")
      .add({
        name: name,
        email: email,
        message: message,
      })
      .then(() => {
        alert("Message has been submitted 😉");
      })
      .catch((error) => {
        alert(error.message);
      });
    setName("");
    setEmail("");
    setMessage("");
  };

  const navigate = useNavigate();
  const handleBack = () => {
    if(props.isLoggedIn) {navigate("/Profile")}
    else {navigate("/")}
  };

  return (
    <div className="aboutus-all">
      <div className="blur">
        <Header
          isLoggedIn={props.isLoggedIn}
          userInfo={props.userInfo}
          setIsLoggedIn={props.setIsLoggedIn}
          setUserInfo={props.setUserInfo}
        />
        <div className="aboutus-container">
          <div class="row">
           <h2><VscClose className="howtouse-back-btn" onClick={handleBack}></VscClose></h2>
            <h2>About us</h2>
            <section className="column">
              <h3>Who we are...</h3>
              <article className="aboutus-text">
                <p>
                We are four software developers. Robin Öberg, Sareh Jalali, Alex Hagdahl and Carl Holmström currently working as consultants for &lt;/salt&gt;.
                </p>
              </article>
              <article className="aboutus-text">
                <h3>What we do...</h3>
                <article className="aboutus-text">
                  <p>
                  Digital signatures is a paradigm shift. The world has changed and the technology is already there but is not widely being used.
                  QSign's vision is to make the use of digital signatures available world-wide, simple and secure. Removing the need to trust each other by placing the trust in mathematics.

                  </p>
                </article>
              </article>
            </section>
            <div class="column">
              <h3>Contact us</h3>
              <form className="contactUs-form" onSubmit={handleSubmit}>
                <label for="fname">Name</label>
                <input
                  type="text"
                  placeholder=" Name.."
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                />
                <label for="lname">Email</label>
                <input
                  type="text"
                  placeholder=" Email.."
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <label for="message">Message</label>
                <textarea
                  placeholder="message..."
                  value={message}
                  onChange={(e) => setMessage(e.target.value)}
                />
                <button className="submit-btn" type="submit">
                  SEND
                </button>
              </form>
              <h3>View project on GitHub</h3>
              <ul className="aboutus-links">
                <li>
                  <a href="https://github.com/Sarehj/Qsign">
                  <BsGithub size={40}/>
                  </a>
                </li>
                <h3>Contact us by email</h3>
                <li>
                <a href="mailto:noreplyqsign@gmail.com"><MdEmail size={40} /></a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AboutUs;
