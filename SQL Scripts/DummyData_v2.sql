use JaaamDb;
-- INSERT USER/ACCOUNT
INSERT INTO Account (Balance) VALUES (0.00);
INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) VALUES ('Deleted', 'User', 'DeletedUser', 'deleteduser@jaaam.com', '0', @@IDENTITY);
INSERT INTO Account (Balance) VALUES (0.00);
INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) VALUES ('Aske', 'Knudsen', 'askeKnudsen', 'askeKnudsen@jaaam.com', '12345', @@IDENTITY);
INSERT INTO Account (Balance) VALUES (0.00);
INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) VALUES ('Anders', 'Jørgensen', 'andersJørgensen', 'andersJørgensen@jaaam.com', '12345', @@IDENTITY);
INSERT INTO Account (Balance) VALUES (0.00);
INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) VALUES ('Mark', 'Sønderby', 'markSønderby', 'markSønderby@jaaam.com', '12345', @@IDENTITY);
INSERT INTO Account (Balance) VALUES (8000.00);
INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) VALUES ('Jacob', 'Roed', 'jacobRoed', 'jacobRoed@jaaam.com', '12345', @@IDENTITY);
INSERT INTO Account (Balance) VALUES (0.00);
INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) VALUES ('Alexander', 'Nygaard', 'alexanderNygaard', 'alexanderNygaard@jaaam.com', '12345', @@IDENTITY);
-- SELECT FirstName, LastName, Username, Email, PhoneNumber, Balance FROM [User] u, Account a WHERE u.Id = @@IDENTITY AND AccountId = a.Id

-- Create FreeAgent team
INSERT INTO Team (Name) VALUES ('Free Agents');

-- INSERT PLAYERS IN TEAMS (Astralis)
DECLARE @TeamId1 int
DECLARE @TeamId2 int
DECLARE @TeamId3 int
DECLARE @TeamId4 int
DECLARE @TeamId5 int
DECLARE @TeamId6 int
DECLARE @TeamId7 int
DECLARE @TeamId8 int



INSERT INTO Team (Name) VALUES ('Astralis');
SELECT @TeamId1 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('device', '22', 'AWPer', @TeamId1);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('dupreeh', '25', 'Entry', @TeamId1);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Xyp9x', '24', 'Support', @TeamId1);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('gla1ve', '25', 'IGLer', @TeamId1);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Magisk', '20', 'Rifle', @TeamId1);

-- INSERT PLAYERS IN TEAMS (North)
INSERT INTO Team (Name) VALUES ('North');
SELECT @TeamId2 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('aizy', '22', 'Rifle', @TeamId2);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('valde', '23', 'Entry', @TeamId2);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Kjaerbye', '21', 'Rifle', @TeamId2);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Cadian', '24', 'IGLer', @TeamId2);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('gade', '20', 'Rifle', @TeamId2);

-- INSERT PLAYERS IN TEAMS (MIBR)
INSERT INTO Team (Name) VALUES ('MIBR');
SELECT @TeamId3 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('FalleN', '27', 'AWPer', @TeamId3);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('fer', '27', 'Rifle', @TeamId3);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('coldzera', '24', 'IGLer', @TeamId3);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Stewie2K', '20', 'Rifle', @TeamId3);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('tarik', '22', 'Rifle', @TeamId3);

-- INSERT PLAYERS IN TEAMS (Fnatic)
INSERT INTO Team (Name) VALUES ('Fnatic');
SELECT @TeamId4 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('KRIMZ', '23', 'Rifle', @TeamId4);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('JW', '21', 'Entry', @TeamId4);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Xizt', '28', 'IGLer', @TeamId4);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('twist', '25', 'AWPer', @TeamId4);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Brollan', '29', 'Rifle', @TeamId4);

-- INSERT PLAYERS IN TEAMS (Liquid)
INSERT INTO Team (Name) VALUES ('Liquid');
SELECT @TeamId5 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('EliGE', '23', 'Rifle', @TeamId5);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('NAF', '21', 'Entry', @TeamId5);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('nitr0', '28', 'IGLer', @TeamId5);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('TACO', '25', 'AWPer', @TeamId5);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Twistzz', '29', 'Rifle', @TeamId5);

-- INSERT PLAYERS IN TEAMS (Nip)
INSERT INTO Team (Name) VALUES ('Nip');
SELECT @TeamId6 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('REZ', '23', 'Rifle', @TeamId6);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('dennis', '21', 'Entry', @TeamId6);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Lekr0', '28', 'IGLer', @TeamId6);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('f0rest', '25', 'AWPer', @TeamId6);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Get_Right', '29', 'Rifle', @TeamId6);

-- INSERT PLAYERS IN TEAMS (NRG)
INSERT INTO Team (Name) VALUES ('NRG');
SELECT @TeamId7 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('Brezhe', '23', 'Rifle', @TeamId7);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('CeRq', '21', 'Entry', @TeamId7);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('FugLy', '28', 'IGLer', @TeamId7);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('nahtE', '25', 'AWPer', @TeamId7);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('daps', '29', 'Rifle', @TeamId7);

-- INSERT PLAYERS IN TEAMS (BIG)
INSERT INTO Team (Name) VALUES ('BIG');
SELECT @TeamId8 = SCOPE_IDENTITY()
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('nex', '23', 'Rifle', @TeamId8);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('tiziaN', '21', 'Entry', @TeamId8);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('smooya', '28', 'IGLer', @TeamId8);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('tabseN', '25', 'AWPer', @TeamId8);
INSERT INTO Player (NickName, Age, Role, TeamId) VALUES('gob b', '29', 'Rifle', @TeamId8);

-- SELECT NickName, Age, Role, Name FROM Player p, Team t, PlayersOnTeam pot WHERE p.Id = pot.PlayerId AND t.Id = pot.TeamId AND p.Id = 1;
-- SELECT p.Id, p.NickName, p.Age, p.Role, t.Name FROM player p INNER JOIN PlayersOnTeam pot on p.Id = pot.PlayerId INNER JOIN Team t on t.Id = pot.TeamId WHERE p.Id = 1

-- INSERT EVENT, MATCH AND MAPS
DECLARE @EventId int
DECLARE @EventId2 int
DECLARE @EventId3 int
DECLARE @EventId4 int
DECLARE @MatchId int
DECLARE @MatchId2 int
DECLARE @MatchId3 int
DECLARE @MatchId4 int
DECLARE @MapId int

INSERT INTO Event (Name, GameName, Type) VALUES ('BlastProSeries', 'CSGO', 'Offline');
SELECT @EventId = SCOPE_IDENTITY()
INSERT INTO Event (Name, GameName, Type) VALUES ('ECS', 'CSGO', 'Offline');
SELECT @EventId3 = SCOPE_IDENTITY()
INSERT INTO Event (Name, GameName, Type) VALUES ('IEM Chicago', 'CSGO', 'Offline');
SELECT @EventId4 = SCOPE_IDENTITY()

INSERT INTO Match (Format, WinnerId, EventId) VALUES ('BO1', -1, @EventId);
SELECT @MatchId = SCOPE_IDENTITY()
INSERT INTO TeamsInMatch VALUES(@TeamId1, @MatchId);
INSERT INTO TeamsInMatch VALUES(@TeamId3, @MatchId);

INSERT INTO Match (Format, WinnerId, EventId) VALUES ('BO1', -1, @EventId3);
SELECT @MatchId3 = SCOPE_IDENTITY()
INSERT INTO TeamsInMatch VALUES(@TeamId2, @MatchId3);
INSERT INTO TeamsInMatch VALUES(@TeamId5, @MatchId3);

INSERT INTO Match (Format, WinnerId, EventId) VALUES ('BO1', -1, @EventId3);
SELECT @MatchId4 = SCOPE_IDENTITY()
INSERT INTO TeamsInMatch VALUES(@TeamId6, @MatchId4);
INSERT INTO TeamsInMatch VALUES(@TeamId7, @MatchId4);

INSERT INTO Match (Format, WinnerId, EventId) VALUES ('BO1', -1, @EventId4);
SELECT @MatchId4 = SCOPE_IDENTITY()
INSERT INTO TeamsInMatch VALUES(@TeamId4, @MatchId4);
INSERT INTO TeamsInMatch VALUES(@TeamId8, @MatchId4);

INSERT INTO Map (Name, IsActive) VALUES('de_dust2', 1);
SELECT @MapId = SCOPE_IDENTITY()
INSERT INTO MapsOnMatch VALUES(@MatchId, @MapId);

INSERT INTO Map (Name, IsActive) VALUES('de_inferno', 1);
SELECT @MapId = SCOPE_IDENTITY()
INSERT INTO MapsOnMatch VALUES(@MatchId, @MapId);
INSERT INTO MapsOnMatch VALUES(@MatchId3, @MapId);


INSERT INTO Map (Name, IsActive) VALUES('de_mirage', 1);
SELECT @MapId = SCOPE_IDENTITY()
INSERT INTO MapsOnMatch VALUES(@MatchId, @MapId);
--INSERT INTO MapsOnMatch VALUES(@MatchId2, @MapId);

INSERT INTO Map (Name, IsActive) VALUES('de_nuke', 1);
SELECT @MapId = SCOPE_IDENTITY()
INSERT INTO MapsOnMatch VALUES(@MatchId3, @MapId);
INSERT INTO MapsOnMatch VALUES(@MatchId4, @MapId);

INSERT INTO Map (Name, IsActive) VALUES('de_cache', 1);
SELECT @MapId = SCOPE_IDENTITY()
INSERT INTO MapsOnMatch VALUES(@MatchId4, @MapId);


-- SELECT m.Name FROM Match ma, Map m, MapsOnMatch mom WHERE ma.Id = mom.MatchId AND m.Id = mom.MapId AND ma.Id = 1;

-- INSERT BETS
DECLARE @BetId int
DECLARE @BomId int

-- Bet on Astralis
INSERT INTO Bet (Amount, Odds, UserId) VALUES(200.00, 2.45, 5);
SELECT @BetId = SCOPE_IDENTITY()
INSERT INTO BetsOnMatch(MatchId, BetId) VALUES(@MatchId, @BetId);
SELECT @BomId = SCOPE_IDENTITY()
INSERT INTO TeamsOnBetsOnMatch(TeamId, BomId) VALUES(@TeamId1, @BomId);

-- Bet on Fnatic
INSERT INTO Bet (Amount, Odds, UserId) VALUES(233.00, 5, 5);
SELECT @BetId = SCOPE_IDENTITY()
INSERT INTO BetsOnMatch(MatchId, BetId) VALUES(@MatchId4, @BetId);
SELECT @BomId = SCOPE_IDENTITY()
INSERT INTO TeamsOnBetsOnMatch(TeamId, BomId) VALUES(@TeamId4, @BomId);

-- Bet on North
INSERT INTO Bet (Amount, Odds, UserId) VALUES(2000.00, 3, 5);
SELECT @BetId = SCOPE_IDENTITY()
INSERT INTO BetsOnMatch(MatchId, BetId) VALUES(@MatchId3, @BetId);
SELECT @BomId = SCOPE_IDENTITY()
INSERT INTO TeamsOnBetsOnMatch(TeamId, BomId) VALUES(@TeamId2, @BomId);



--SELECT m.Id AS MatchNr, m.Format, b.Amount, b.Odds
--FROM [User] u
--INNER JOIN BetsOnUser bou on u.Id = bou.UserId
--INNER JOIN Bet b on b.Id = bou.BetId
--INNER JOIN BetsOnMatch bom on bou.BetId = bom.BetId
--INNER JOIN Match m on m.Id = bom.MatchId 
--WHERE u.Id = 1

