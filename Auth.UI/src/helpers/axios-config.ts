import axios, { AxiosError, AxiosInstance } from "axios"

export function AxiosConfig(): AxiosInstance {
    var token = localStorage.getItem('token')
    const auth: any = {}

    if (token)
        auth.Authorization = `Bearer ${token}`

    const instance = axios.create({
        baseURL: "https://localhost:7038/api",
        headers: {
            ...auth,
            "Content-Type": "application/json",
        }
    })

    instance.interceptors.request.use(
        (request) => {
            return request;
        },

        (error: AxiosError) => {
            throw error;
        }
    )
    return instance;
}