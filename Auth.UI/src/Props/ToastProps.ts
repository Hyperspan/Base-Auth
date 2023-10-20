export interface ToastProps {
    heading: string
    type: "info" | "danger" | "success" | "warning"
    content: string
    id?: string
}
