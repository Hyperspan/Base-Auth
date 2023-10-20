import { useSelector } from "react-redux"
import { RootState } from "../../helpers/store"
import { Toast } from "./Toast"

export default function ToastContainer() {
  const toasts = useSelector((rootState: RootState) => rootState.toastReducer)
  return (
    <div className="toast-container">
      {toasts.map((toast) => (
        <Toast {...toast} key={toast.id} />
      ))}
    </div>
  )
}
