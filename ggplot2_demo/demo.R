
name <- "demo.csv"

n <- strsplit(name,'.',fixed = TRUE)[[1]]
fname <- n[1:1]
library(ggplot2)
dt <- read.table(name, header=TRUE, sep=",", na.strings="NA", dec=".", strip.white=TRUE)
dt$SSD <- factor(dt$SSD)  
str(dt)
filtertps <- function(y) { subset(y, TPS>100)  }
dd=subset(filtertps(dt))
m <- ggplot(dd, aes(x=SSD, y=TPS)) + geom_jitter(size=2, aes(color=SSD)) + labs(x='', y='TPS (Transaction per second)') +  guides(color=FALSE)
library(plyr)
dd.mean <- ddply(dd,'SSD',summarize, TPS = round(mean(TPS), 2), RT = round(mean(RT), 2))
m + geom_text(data=dd.mean, aes(x=SSD, label=TPS)) + theme(axis.text.x=element_text(angle=45,vjust=1,hjust=1,color="black",size="10"))
ggsave(paste(n[1], "png", sep=".") ,width=8, height=5, dpi=400)

