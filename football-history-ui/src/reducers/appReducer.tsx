import { LeagueTableAction, leagueTableReducer, LeagueTableState } from "./leagueTableReducer";
import { ClubsAction, clubsReducer, ClubsState } from "./clubsReducer";

export interface AppState {
  leagueTableState: LeagueTableState;
  clubsState: ClubsState;
}
export type AppAction = LeagueTableAction | ClubsAction;

const appReducer = (prevState: AppState | undefined, action: AppAction): AppState => {
  return {
    leagueTableState: leagueTableReducer(prevState?.leagueTableState, action),
    clubsState: clubsReducer(prevState?.clubsState, action),
  };
};

export { appReducer };
