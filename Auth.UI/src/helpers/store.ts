import { configureStore } from "@reduxjs/toolkit";
import ToastReducer from "../redux/Reducers/ToastReducer";
export const store = configureStore({
    reducer: {
        toastReducer: ToastReducer
    },
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;