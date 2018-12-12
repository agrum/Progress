var projectile = {};

projectile.direction = {}
projectile.direction.Self = "Self"
projectile.direction.Target = "Target"
projectile.direction.SelfTotarget = "SelfTotarget"
projectile.direction.TargetToSelf = "TargetToSelf" 

projectile.shape = {}
projectile.shape.Circle = "Circle"
projectile.shape.Rect5To1 = "5To1"
projectile.shape.Rect3To1 = "3To1"
projectile.shape.Rect1To1 = "1To1"
projectile.shape.Rect1To3 = "1To3"
projectile.shape.Rect1To4 = "1To5"

projectile.collideWith = {}
projectile.collideWith.All = "All"
projectile.collideWith.Ally = "Ally"
projectile.collideWith.AllyPlayer = "AllyPlayer"
projectile.collideWith.Enemy = "Enemy"
projectile.collideWith.EnemyPlayer = "EnemyPlayer"
projectile.collideWith.Environment = "Environment"
projectile.collideWith.Projectile = "Projectile"

module.exports = projectile