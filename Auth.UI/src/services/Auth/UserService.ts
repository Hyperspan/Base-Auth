import { AxiosConfig } from "../../helpers/axios-config";
import RegisterUserRequest from "../../modals/Auth/RegisterUserRequest";
import { ApiResponse } from "../../modals/ApiResponse";
import RegisterUserResponse from "../../modals/Auth/RegisterUserResponse";
import LoginUserRequest from "../../modals/Auth/LoginUserRequest";
import { LoginUserResponse } from "../../modals/Auth/LoginUserResponse";

export function RegisterUser(registerUserRequest: RegisterUserRequest): Promise<ApiResponse<RegisterUserResponse>> {
    return AxiosConfig().post<ApiResponse<RegisterUserResponse>>("/account/user/register", registerUserRequest).then(x => x.data)
}

export function LoginUser(registerUserRequest: LoginUserRequest): Promise<ApiResponse<LoginUserResponse>> {
    return AxiosConfig().post<ApiResponse<LoginUserResponse>>("/account/user/login", registerUserRequest).then(x => x.data)
}