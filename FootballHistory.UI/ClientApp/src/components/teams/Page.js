import React, { useState } from 'react';
import Filter from "./Filter";

function Page() {
  const [selectedTeam, setSelectedTeam] = useState('Test Team');

  return (
      <div>
        <h1>{selectedTeam}</h1>
          <Filter updateSelectedTeam={(team) => setSelectedTeam(team)} selectedTeam={selectedTeam}/>
      </div>
  );
}

export default Page;
