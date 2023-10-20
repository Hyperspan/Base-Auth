import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { v4 as uuidv4 } from 'uuid';
import { ToastProps } from "../../Props/ToastProps";

var state: ToastProps[] = []


const ToastStateSlice = createSlice({
    name: "ToastState",
    initialState: state,
    reducers: {
        AddToast(state: ToastProps[], payload: PayloadAction<ToastProps>) {
            payload.payload.id = uuidv4()
            state = [...state, payload.payload]
        },
        RemoveToast(state: ToastProps[], payload: PayloadAction<string>) {
            var nState = state;
            var index = nState.findIndex(x => x.id == payload.payload)
            nState.slice(index, 1);
            state = nState
            console.log(state);

        }

    },
});

export const {
    AddToast,
    RemoveToast
} = ToastStateSlice.actions;

export default ToastStateSlice.reducer;