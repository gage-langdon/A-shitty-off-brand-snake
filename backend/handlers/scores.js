const UserModel = require("../models/user");

module.exports.getScores = async () => {
  const User = await UserModel();
  const users = await User.getUsersSortedByScore(10);
  const leaderboard = users.map(({ name, score }, i) => ({
    name,
    score,
    rank: i + 1,
  }));

  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        scores: leaderboard,
      },
      null,
      2
    ),
  };
};

module.exports.postScore = async (event) => {
  try {
    const { score, name } = JSON.parse(event.body);

    const User = await UserModel();
    const newUser = User.create({ name, score });

    return {
      statusCode: 200,
      body: JSON.stringify(newUser, null, 2),
    };
  } catch (e) {
    console.log("Failed to post score. Error: ", e);

    return {
      statusCode: 400,
      body: JSON.stringify({ message: "Failed to post score." }, null, 2),
    };
  }
};
