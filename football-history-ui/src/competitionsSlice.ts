import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { Competition } from "./App/shared/useFetchCompetitions";

type CompetitionState = {
  status: "UNLOADED" | "LOADING" | "LOADED" | "LOAD_FAILED";
  competitions: Competition[];
  selectedCompetition: Competition | undefined;
  error: string | undefined;
};

const initialState: CompetitionState = {
  status: "UNLOADED",
  competitions: [],
  selectedCompetition: undefined,
  error: undefined,
};

export const fetchCompetitionsBySeasonId = createAsyncThunk(
  "competitions/fetchBySeasonId",
  async (seasonId: number) => {
    const response = await fetch(`https://localhost:5001/api/v2/competitions/season/${seasonId}`);
    return (await response.json()).result as Competition[];
  }
);

export const competitionsSlice = createSlice({
  name: "competitions",
  initialState,
  reducers: {
    selectCompetition: (state, action) => {
      state.selectedCompetition = action.payload;
    },
    clearSelectedCompetition: (state) => {
      state.selectedCompetition = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchCompetitionsBySeasonId.pending, (state, _) => {
      state.status = "LOADING";
    });
    builder.addCase(fetchCompetitionsBySeasonId.fulfilled, (state, action) => {
      state.status = "LOADED";
      state.competitions = action.payload;
      state.selectedCompetition = action.payload.filter(
        (x) => x.level === state.selectedCompetition?.level
      )[0];
    });
    builder.addCase(fetchCompetitionsBySeasonId.rejected, (state, action) => {
      state.status = "LOAD_FAILED";
      state.error = action.error.message;
    });
  },
});

export const { selectCompetition, clearSelectedCompetition } = competitionsSlice.actions;

export default competitionsSlice.reducer;
