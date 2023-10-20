import { RegisterOptions, useForm } from "react-hook-form"
import LoginUserRequest from "../../modals/Auth/LoginUserRequest"
import { LoginUser } from "../../services/Auth/UserService"
import { useDispatch } from "react-redux"
import { AddToast } from "../../redux/Reducers/ToastReducer"
import { ToastProps } from "../../Props/ToastProps"
import { useNavigate } from "react-router-dom"

export default function Login() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginUserRequest>()
  const dispatch = useDispatch()
  const navigate = useNavigate()

  function onFormSubmit(formValues: LoginUserRequest) {
    LoginUser(formValues).then((response) => {
      if (response.succeeded) {
        var toast: ToastProps = {
          content: "Successfully Logged in!",
          heading: "Login Success",
          type: "success",
        }
        localStorage.setItem("userData", JSON.stringify(response.data))
        dispatch(AddToast(toast))
        navigate("/")
      }
    })
  }

  return (
    <div className="container d-flex align-content-center justify-content-center login-container">
      <form className="login-form" onSubmit={handleSubmit(onFormSubmit)}>
        <h3 className="text-center">Login To ERP</h3>
        <div className="row">
          <div className="col-md-12 mb-3">
            <label htmlFor="userName">Email</label>
            <input type="text" id="userName" className="form-control" {...register("userName", userNameValidations)} />
            <div className="invalid-feedback">{errors.userName && errors.userName.message && errors.userName.message.toString()}</div>
          </div>
          <div className="col-md-12 mb-3">
            <label htmlFor="password">Password</label>
            <input type="password" id="password" className="form-control" {...register("password", passwordValidations)} />
            <div className="invalid-feedback">{errors.password && errors.password.message && errors.password.message.toString()}</div>
          </div>
          <div className="d-flex justify-content-end col-md-12 mt-3">
            <button type="submit" className="btn btn-success">
              Login
            </button>
          </div>
        </div>
      </form>
    </div>
  )
}

const userNameValidations: RegisterOptions = {
  required: {
    value: true,
    message: "Email Address is required",
  },
}

const passwordValidations: RegisterOptions = {
  required: {
    value: true,
    message: "Password is required",
  },
}
