const mongoose = require("mongoose");
const config = require("../config");

module.exports.connect = async (url = config.mongoURI) => {
  await mongoose.connect(url);
};
