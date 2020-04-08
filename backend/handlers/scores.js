const UserModel = require("../models/user");
const { Response } = require("../utils/response");

module.exports.getScores = async (event, context, callback) => {
  const res = Response(callback);
  try {
    const User = await UserModel();
    const users = await User.getUsersSortedByScore(10);
    const scores = users.map(({ name, score }, i) => ({
      name,
      score,
      rank: i + 1,
    }));

    res.send({
      scores,
    });
  } catch (e) {
    console.error("Failed to get scores. Error: ", e);
    res.error({ message: "Failed to get scores." });
  }
};

module.exports.postScore = async (event) => {
  const res = Response(callback);
  try {
    const { score, name } = JSON.parse(event.body);

    const User = await UserModel();
    const newUser = await User.create({ name, score });

    res.send({ ok: true });
  } catch (e) {
    console.error("Failed to post score. Error: ", e);
    res.error({ message: "Failed to post score." });
  }
};
