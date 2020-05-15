import { LeagueTableAction, leagueTableReducer, LeagueTableState } from "./leagueTableReducer";
import { ClubsAction, clubsReducer, ClubsState } from "./clubsReducer";
import { SeasonsAction, seasonsReducer, SeasonsState } from "./seasonsReducer";

export interface AppState {
  leagueTableState: LeagueTableState;
  clubsState: ClubsState;
  seasonsState: SeasonsState;
}
export type AppAction = LeagueTableAction | ClubsAction | SeasonsAction;

const appReducer = (prevState: AppState | undefined, action: AppAction): AppState => {
  return {
    leagueTableState: leagueTableReducer(prevState?.leagueTableState, action),
    clubsState: clubsReducer(prevState?.clubsState, action),
    seasonsState: seasonsReducer(prevState?.seasonsState, action),
  };
};

export { appReducer };
