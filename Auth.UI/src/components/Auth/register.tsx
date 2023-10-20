import { FieldValues, RegisterOptions, useForm } from "react-hook-form"
import { RegisterUser } from "../../services/Auth/UserService"
import RegisterUserRequest from "../../modals/Auth/RegisterUserRequest"
import ToastContainer from "../Layout/ToastContainer"
import { useDispatch } from "react-redux"
import { AddToast } from "../../redux/Reducers/ToastReducer"
import { ToastProps } from "../../Props/ToastProps"
import { useNavigate } from "react-router-dom"

export default function Register() {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<RegisterUserRequest>()

  function onFormSubmit(formValues: RegisterUserRequest) {
    RegisterUser(formValues).then((x) => {
      if (x.succeeded) {
        var toast: ToastProps = {
          content: "You have registered successfully. Please check your email for further steps.",
          heading: "Registration Successful",
          type: "success",
        }
        dispatch(AddToast(toast))
        reset()
        navigate("/")
      }
      if (!x.data) return
    })
  }

  return (
    <div className="container d-flex align-content-center justify-content-center login-container">
      <ToastContainer></ToastContainer>
      <form className="login-form" onSubmit={handleSubmit(onFormSubmit)}>
        <h3 className="text-center">Register To ERP</h3>
        <div className="row">
          <div className="col-md-12 mb-3">
            <label htmlFor="email">Email</label>
            <input type="email" id="email" className="form-control" {...register("email", emailValidations)} />
            <div className="invalid-feedback">{errors.email && errors.email.message && errors.email.message.toString()}</div>
          </div>
          <div className="col-md-12 mb-3">
            <label htmlFor="username">User Name</label>
            <input type="text" required id="username" className="form-control" {...register("userName", userNameValidations)} />
            <div className="invalid-feedback">{errors.userName && errors.userName.message && errors.userName.message.toString()}</div>
          </div>
          <div className="col-md-12 mb-3">
            <label htmlFor="mobileNumber">Mobile Number</label>
            <input type="text" required id="mobileNumber" className="form-control" {...register("mobileNumber", mobileNumberValidations)} />
            <div className="invalid-feedback">{errors.mobileNumber && errors.mobileNumber.message && errors.mobileNumber.message.toString()}</div>
          </div>
          <div className="col-md-12 mb-3">
            <label htmlFor="password">Password</label>
            <input type="password" required id="password" className="form-control" {...register("password", passwordValidations)} />
            <div className="invalid-feedback">{errors.password && errors.password.message && errors.password.message.toString()}</div>
          </div>
          <div className="col-md-12 mb-3">
            <label htmlFor="c-password">Confirm Password</label>
            <input type="password" required id="c-password" className="form-control" {...register("confirmPassword", confirmPasswordValidations)} />
            <div className="invalid-feedback">{errors.confirmPassword && errors.confirmPassword.message && errors.confirmPassword.message.toString()}</div>
          </div>
          <div className="d-flex justify-content-end col-md-12 mt-3">
            <button type="submit" className="btn btn-success">
              Register
            </button>
          </div>
        </div>
      </form>
    </div>
  )
}

const emailValidations: RegisterOptions = {
  pattern: {
    value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
    message: "Please enter a valid Email address",
  },
  required: {
    value: true,
    message: "Email Address is required",
  },
}

const userNameValidations: RegisterOptions = {
  required: {
    value: true,
    message: "Username is required",
  },
  maxLength: {
    value: 50,
    message: "Username cannot be more than 50 characters.",
  },
}

const mobileNumberValidations: RegisterOptions = {
  required: {
    value: true,
    message: "Mobile Number is required",
  },
  maxLength: {
    value: 15,
    message: "Mobile Number cannot be more than 50 characters.",
  },
}

const passwordValidations: RegisterOptions = {
  required: {
    value: true,
    message: "Password is required",
  },
}

const confirmPasswordValidations: RegisterOptions = {
  required: {
    value: true,
    message: "Confirm Password is required",
  },
  validate: {
    compareFields: (value: string, formValues: FieldValues) => {
      let validation = value === formValues["password"]
      return validation ? validation : "Confirm Password does not match Password"
    },
  },
}
