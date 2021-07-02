import { configureStore } from "@reduxjs/toolkit";
import seasonReducer from "./App/shared/seasonsSlice";
import competitionReducer from "./App/shared/competitionsSlice";
import teamReducer from "./App/shared/teamsSlice";

const reduxStore = configureStore({
  reducer: {
    season: seasonReducer,
    competition: competitionReducer,
    team: teamReducer,
  },
});

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;

export default reduxStore;
