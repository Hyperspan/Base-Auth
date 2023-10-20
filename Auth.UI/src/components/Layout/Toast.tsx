import { faClose } from "@fortawesome/free-solid-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { useDispatch } from "react-redux"
import { ToastProps } from "../../Props/ToastProps"
import { RemoveToast } from "../../redux/Reducers/ToastReducer"

export function Toast(props: ToastProps) {
  var dispatch = useDispatch()

  function onCloseToast() {
    if (props.id) dispatch(RemoveToast(props.id))
  }

  return (
    <div className={`toast show toast-${props.type}`}>
      <div className="toast-heading">
        <h4>
          <i className="fa-solid fa-user me-2"></i>
          {props.heading}
        </h4>
        <button className="btn" onClick={onCloseToast}>
          <FontAwesomeIcon icon={faClose}></FontAwesomeIcon>
        </button>
      </div>
      <div className="toast-body">
        <p>{props.content}</p>
      </div>
    </div>
  )
}
