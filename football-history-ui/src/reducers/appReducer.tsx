import { LeagueTableAction, leagueTableReducer, LeagueTableState } from "./leagueTableReducer";

export interface AppState {
  leagueTableState: LeagueTableState;
}
export type AppAction = LeagueTableAction;

const appReducer = (prevState: AppState | undefined, action: AppAction): AppState => {
  return {
    leagueTableState: leagueTableReducer(prevState?.leagueTableState, action),
  };
};

export { appReducer };
