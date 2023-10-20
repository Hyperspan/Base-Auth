export default function Navbar() {
  return (
    <>
      <nav className="navbar navbar-expand-sm bg-body-tertiary">
        <div className="container-fluid">
          <div className="d-flex">
            <button className="btn me-3" type="button">
              <span className="navbar-toggler-icon"></span>
            </button>
            <a className="navbar-brand" href="/">
              Logo
            </a>
          </div>

          <ul className="navbar-nav ms-auto me-3 mb-2 mb-lg-0">
            <li className="nav-item">
              <img className="profile-picture" alt="Profile Picture" src="/profile.jpg" />
            </li>
          </ul>
        </div>
      </nav>
    </>
  )
}
