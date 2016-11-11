#import <Foundation/Foundation.h>

@interface NativeLocale : NSObject
{
    
}

- (id)init;
- (NSString *)getLocale;

@end

@implementation NativeLocale

- (id)init
{
    self = [super init];
    return self;
}

- (NSString *)getLocale
{
    NSString *lang = [[NSLocale currentLocale] objectForKey:NSLocaleLanguageCode];
    return lang;
}

@end


// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

static NativeLocale* instance = nil;

extern "C" {

	const char* _CNativeLocaleGetLocale()
	{
        if(instance == nil)
            instance = [[NativeLocale alloc] init];
        
		return MakeStringCopy([[instance getLocale] UTF8String]);
	}
    
}
