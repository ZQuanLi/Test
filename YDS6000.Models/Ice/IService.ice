module DataProcess {
    interface communication {
	   bool SendCollectVal(string content);
	   string GetCollectVal(string strKey);
       string send(string content);
	   string receive(string command);
    };
};