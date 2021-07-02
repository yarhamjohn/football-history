import { configureStore } from "@reduxjs/toolkit";
import seasonReducer from "./App/shared/seasonsSlice";
import competitionReducer from "./App/shared/competitionsSlice";

const reduxStore = configureStore({
  reducer: {
    season: seasonReducer,
    competition: competitionReducer,
  },
});

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;

export default reduxStore;
