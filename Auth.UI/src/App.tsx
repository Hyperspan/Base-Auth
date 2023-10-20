import { BrowserRouter, Route, Routes } from "react-router-dom"
import Register from "./components/Auth/register"
import "./css"
import "bootstrap/dist/css/bootstrap.min.css"
import Login from "./components/Auth/Login"
import Layout from "./components/Layout/layout"

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/register" element={<Register />}></Route>
        <Route path="/login" element={<Login />}></Route>
        <Route path="/roles" element={<Login />}></Route>
        <Route path="/user-roles" element={<Login />}></Route>
      </Routes>
      <Routes>
        <Route path="/" element={<Layout />}></Route>
      </Routes>
    </BrowserRouter>
  )
}
