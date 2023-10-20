import ReactDOM from "react-dom/client"
import App from "./App.tsx"
import "./index.css"
import { Provider } from "react-redux"
import { store } from "./helpers/store.ts"
import ToastContainer from "./components/Layout/ToastContainer.tsx"

ReactDOM.createRoot(document.getElementById("root")!).render(
  <Provider store={store}>
    <App />
    <ToastContainer />
  </Provider>
)
