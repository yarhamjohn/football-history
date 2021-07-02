import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

export type Team = {
  id: number;
  name: string;
  abbreviation: string;
  notes: string | null;
};

type TeamState = {
  status: "UNLOADED" | "LOADING" | "LOADED" | "LOAD_FAILED";
  teams: Team[];
  selectedTeam: Team | undefined;
  error: string | undefined;
};

const initialState: TeamState = {
  status: "UNLOADED",
  teams: [],
  selectedTeam: undefined,
  error: undefined,
};

export const fetchTeams = createAsyncThunk("teams/fetchTeams", async () => {
  const response = await fetch("https://localhost:5001/api/v2/teams");
  return (await response.json()).result as Team[];
});

export const teamsSlice = createSlice({
  name: "teams",
  initialState,
  reducers: {
    selectTeam: (state, action) => {
      state.selectedTeam = action.payload;
    },
    clearSelectedTeam: (state) => {
      state.selectedTeam = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchTeams.pending, (state, _) => {
      state.status = "LOADING";
    });
    builder.addCase(fetchTeams.fulfilled, (state, action) => {
      state.status = "LOADED";
      state.teams = action.payload;
    });
    builder.addCase(fetchTeams.rejected, (state, action) => {
      state.status = "LOAD_FAILED";
      state.error = action.error.message;
    });
  },
});

export const { selectTeam, clearSelectedTeam } = teamsSlice.actions;

export default teamsSlice.reducer;
