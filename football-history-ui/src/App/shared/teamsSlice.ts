import { createAsyncThunk, createSelector, createSlice, PayloadAction } from "@reduxjs/toolkit";

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
    setSelectedTeam: (state, action: PayloadAction<Team>) => {
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

const selectTeams = (state: TeamState) => state.teams;
const selectTeamId = (state: TeamState, id: number) => id;
export const selectTeamById = createSelector(
  [selectTeams, selectTeamId],
  (teams, id) => teams.filter((x) => x.id === id)[0]
);

export const { setSelectedTeam, clearSelectedTeam } = teamsSlice.actions;

export default teamsSlice.reducer;
