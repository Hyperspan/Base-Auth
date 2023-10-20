export interface ApiResponse<T = any> {

    errorCode: string,
    message: string,
    succeeded: boolean,
    data: T | null

}
