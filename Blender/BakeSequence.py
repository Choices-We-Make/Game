import bpy

for i in range(1, 30):
    print("Baking "+str(i)+"...")
    bpy.context.scene.frame_set(i)
    bpy.ops.object.bake(type='COMBINED')