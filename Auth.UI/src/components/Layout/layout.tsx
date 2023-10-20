import { Outlet } from "react-router-dom"
import { Footer, Navbar, Sidebar } from "."

export default function layout() {
  return (
    <>
      <Navbar />
      <div className="main-container">
        <Sidebar />
        <main>
          <Outlet />
        </main>
      </div>
      <Footer />
    </>
  )
}
